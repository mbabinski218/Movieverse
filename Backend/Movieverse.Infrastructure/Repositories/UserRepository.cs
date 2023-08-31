using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Resources;
using Movieverse.Contracts.DataTransferObjects.User;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Models;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.Common.Types;
using Movieverse.Domain.ValueObjects;
using Movieverse.Domain.ValueObjects.Id;
using Movieverse.Infrastructure.Authentication;
using Movieverse.Infrastructure.Common;
using Movieverse.Infrastructure.Persistence;

namespace Movieverse.Infrastructure.Repositories;

public sealed class UserRepository : IUserRepository
{
	private readonly ILogger<UserRepository> _logger;
	private readonly AppDbContext _dbContext;
	private readonly UserManager<User> _userManager;
	private readonly RoleManager<IdentityUserRole> _roleManager;
	private readonly ITokenProvider _tokenProvider;
	private readonly GoogleAuthentication _googleAuthentication;
	private readonly FacebookAuthentication _facebookAuthentication;

	public UserRepository(ILogger<UserRepository> logger, AppDbContext dbContext, UserManager<User> userManager, 
		RoleManager<IdentityUserRole> roleManager, ITokenProvider tokenProvider, 
		GoogleAuthentication googleAuthentication, FacebookAuthentication facebookAuthentication)
	{
		_dbContext = dbContext;
		_userManager = userManager;
		_roleManager = roleManager;
		_tokenProvider = tokenProvider;
		_googleAuthentication = googleAuthentication;
		_facebookAuthentication = facebookAuthentication;
		_logger = logger;
	}

	public async Task<Result<User>> FindByIdAsync(AggregateRootId id, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Find user with id: {id}", id.Value);
		
		var user = await _dbContext.Users.FindAsync(new object?[] { id.Value }, cancellationToken).ConfigureAwait(false);
		return user is null ? Error.NotFound(UserResources.UserDoesNotExist) : user;
	}

	public async Task<Result<User>> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Find user with email: {email}", email);
		
		var user = await _userManager.FindByEmailAsync(email).ConfigureAwait(false);
		return user is null ? Error.NotFound(UserResources.UserDoesNotExist) : user;
	}

	public async Task<Result<string>> GenerateEmailConfirmationTokenAsync(User user, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Generate email confirmation token for user with id: {id}", user.Id);
		
		var token = await _userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);
		return token;
	}
	
	public async Task<Result<string>> RegisterAsync(User user, string? password, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Register user with email: {email}", user.Email);

		IdentityResult result;
		if (password is null)
		{
			result = await _userManager.CreateAsync(user).ConfigureAwait(false);
		}
		else
		{
			result = await _userManager.CreateAsync(user, password).ConfigureAwait(false);
		}
		if (!result.Succeeded)
		{
			Helper.LogError(_logger, result);
			return Error.Invalid(UserResources.FailedToRegisterUser);
		}
		
		result = await _userManager.AddToRoleAsync(user, UserRole.User.ToString()).ConfigureAwait(false);
		if (!result.Succeeded)
		{
			Helper.LogError(_logger, result);
			return Error.Invalid(UserResources.FailedToRegisterUser);
		}

		var claims = CreateClaims(user);
		result = await _userManager.AddClaimsAsync(user, claims).ConfigureAwait(false);
		if (!result.Succeeded)
		{
			Helper.LogError(_logger, result);
			return Error.Invalid(UserResources.FailedToRegisterUser);
		}

		return await GenerateEmailConfirmationTokenAsync(user, cancellationToken).ConfigureAwait(false);
	}

	public async Task<Result> ConfirmEmailAsync(User user, string token, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Confirm email for user with id: {id}", user.Id);
		
		var result = await _userManager.ConfirmEmailAsync(user, token).ConfigureAwait(false);
		if (!result.Succeeded)
		{
			Helper.LogError(_logger, result);
			return Error.Invalid(UserResources.FailedToRegisterUser);
		}

		return Result.Ok();
	}

	public async Task<Result> UpdateAsync(User user, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Update user with id: {id}", user.Id);
		
		_dbContext.Users.Update(user);
		return await Task.FromResult(Result.Ok());
	}
	
	public async Task<Result<TokensDto>> LoginAsync(User user, string password, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Login user with id: {id}", user.Id);

		if (!await _userManager.IsEmailConfirmedAsync(user).ConfigureAwait(false))
		{
			return Error.Unauthorized(UserResources.EmailIsNotConfirmed);
		}

		if (!await _userManager.CheckPasswordAsync(user, password).ConfigureAwait(false))
		{
			return Error.Unauthorized(UserResources.InvalidPassword);
		}
		
		var authClaims = await GetClaims(user).ConfigureAwait(false);
		if (authClaims.IsUnsuccessful)
		{
			return authClaims.Error;
		}
		
		await RemoveTokens(user, cancellationToken).ConfigureAwait(false);
		
		var accessToken = _tokenProvider.GenerateAccessToken(authClaims.Value);
		var refreshToken = _tokenProvider.GenerateRefreshToken();

		await _userManager.SetAuthenticationTokenAsync(user, GrantType.Password.ToStringFast(), "RefreshToken", refreshToken).ConfigureAwait(false);

		return new TokensDto(accessToken, refreshToken);
	}
	
	private static readonly Func<AppDbContext, string, Task<IdentityUserToken<Guid>?>> getUserTokenByRefreshTokenAsync = 
		EF.CompileAsyncQuery((AppDbContext context, string refreshToken) =>
			context.UserTokens.FirstOrDefault(t => t.Value == refreshToken));
	
	public async Task<Result<User>> FindByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
	{
		var userToken = await getUserTokenByRefreshTokenAsync(_dbContext, refreshToken).ConfigureAwait(false);
		if (userToken is null)
		{
			return Error.NotFound(UserResources.UserDoesNotExist);
		}
		
		return await FindByIdAsync(userToken.UserId, cancellationToken).ConfigureAwait(false);
	}
	
	public async Task<Result<TokensDto>> LoginWithRefreshTokenAsync(User user, string refreshToken, CancellationToken cancellationToken = default)
	{
		foreach(var authenticator in GrantTypeExtensions.GetNames())
		{
			var userRefreshToken = await _userManager.GetAuthenticationTokenAsync(user, authenticator, "RefreshToken").ConfigureAwait(false);

			if(userRefreshToken is null) continue;
			
			if (userRefreshToken != refreshToken)
			{
				await _userManager.RemoveAuthenticationTokenAsync(user, authenticator, "RefreshToken").ConfigureAwait(false);
				return Error.Unauthorized(UserResources.InvalidRefreshToken);
			}
			
			await RemoveTokens(user, cancellationToken).ConfigureAwait(false);

			var authClaims = await GetClaims(user).ConfigureAwait(false);
			if (authClaims.IsUnsuccessful)
			{
				return authClaims.Error;
			}
			
			var accessToken = _tokenProvider.GenerateAccessToken(authClaims.Value);
			var newRefreshToken = _tokenProvider.GenerateRefreshToken();

			await _userManager.RemoveAuthenticationTokenAsync(user, authenticator, "RefreshToken").ConfigureAwait(false);
			await _userManager.SetAuthenticationTokenAsync(user, GrantType.RefreshToken.ToStringFast(), "RefreshToken", newRefreshToken).ConfigureAwait(false);

			return new TokensDto(accessToken, newRefreshToken);
		}
		
		return Error.Unauthorized(UserResources.FailedToLoginWithRefreshToken);
	}
	
	public async Task<Result<TokensDto>> LoginWithGoogleAsync(string idToken, CancellationToken cancellationToken = default)
	{
		var googleUser = await _googleAuthentication.AuthenticateAsync(idToken).ConfigureAwait(false);
		if (googleUser.IsUnsuccessful)
		{
			_logger.LogError("Failed to authenticate via Google");
			return googleUser.Error;
		}
		
		var user = _googleAuthentication.GetUser(googleUser.Value.Payload);
		
		if (!googleUser.Value.IsRegistered)
		{
			var token = await RegisterAsync(user, null, cancellationToken).ConfigureAwait(false);

			if (token.IsUnsuccessful)
			{
				_logger.LogError("Failed to register user");
				return token.Error;
			}

			if (_googleAuthentication.IsEmailVerified(googleUser.Value.Payload))
			{
				await _userManager.ConfirmEmailAsync(user, token.Value);
			}
		}
		
		var authClaims = await GetClaims(user).ConfigureAwait(false);
		if (authClaims.IsUnsuccessful)
		{
			return authClaims.Error;
		}
		
		await RemoveTokens(user, cancellationToken).ConfigureAwait(false);
		
		var accessToken = _tokenProvider.GenerateAccessToken(authClaims.Value);
		var refreshToken = _tokenProvider.GenerateRefreshToken();

		await _userManager.SetAuthenticationTokenAsync(user, GrantType.Google.ToStringFast(), "RefreshToken", refreshToken).ConfigureAwait(false);

		return new TokensDto(accessToken, refreshToken);
	}

	public Task<Result<TokensDto>> LoginWithFacebookAsync(string idToken, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	public async Task<Result> LogoutAsync(User user, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Logout user with id: {id}", user.Id);

		return await RemoveTokens(user, cancellationToken).ConfigureAwait(false);
	}

	public async Task<Result<Information>> GetInformationAsync(AggregateRootId id, CancellationToken cancellationToken = default)
	{
		var user = await FindByIdAsync(id, cancellationToken).ConfigureAwait(false);
		if (user.IsUnsuccessful)
		{
			return user.Error;
		}
		
		return user.Value.Information;
	}

	public async Task<Result> AddPersonalityAsync(AggregateRootId id, AggregateRootId personId, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Add personality with id: {personId} to user with id: {id}", personId, id);
		
		var user = await FindByIdAsync(id, cancellationToken).ConfigureAwait(false);
		if(user.IsUnsuccessful)
		{
			return user.Error;
		}
		if(user.Value.PersonId is not null)
		{
			return Error.Invalid(UserResources.UserAlreadyHavePersonality);
		}
		
		user.Value.PersonId = personId;
		return Result.Ok();
	}

	public async Task<Result> AddRoleAsync(User user, UserRole role, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Add role {role} to user with id: {id}", role, user.Id);
		
		var result = await _userManager.AddToRoleAsync(user, role.ToString()).ConfigureAwait(false);
		return result.Succeeded ? Result.Ok() : Error.Invalid(UserResources.FailedToAddRole);
	}

	private async Task<Result> RemoveTokens(User user, CancellationToken cancellationToken = default)
	{
		foreach (var authenticator in GrantTypeExtensions.GetNames())
		{
			await _userManager.RemoveAuthenticationTokenAsync(user, authenticator, "RefreshToken").ConfigureAwait(false);
		}
		
		return Result.Ok();
	}
	
	private static IEnumerable<Claim> CreateClaims(User user)
	{
		var claims = new List<Claim>
		{
			new(ClaimNames.id, user.Id.ToString()),
			new(ClaimNames.email, user.Email!),
			new(ClaimNames.displayName, user.Information.FirstName ?? user.UserName!),
			new(ClaimNames.age, user.Information.Age.ToString()),
		};
		return claims;
	} 
	
	private async Task<Result<List<Claim>>> GetClaims(User user)
	{
		var authClaims = (await _userManager.GetClaimsAsync(user).ConfigureAwait(false)).ToList();

		foreach (var userRole in await _userManager.GetRolesAsync(user).ConfigureAwait(false))
		{
			var role = await _roleManager.FindByNameAsync(userRole).ConfigureAwait(false);

			if (role is null)
			{
				_logger.LogError("Role {role} does not exist", userRole);
				return Error.Invalid(UserResources.CloudNotSignIn);
			}
			
			var claims = await _roleManager.GetClaimsAsync(role).ConfigureAwait(false);
			authClaims.AddRange(claims);
		}

		return authClaims;
	}
}