﻿using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.DataTransferObjects.User;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Models;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.Common.Types;
using Movieverse.Domain.Entities;
using Movieverse.Domain.ValueObjects;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;
using Movieverse.Infrastructure.Authentication;
using Movieverse.Infrastructure.Common;
using Movieverse.Infrastructure.Persistence;

namespace Movieverse.Infrastructure.Repositories;

public sealed class UserRepository : IUserRepository
{
	private readonly ILogger<UserRepository> _logger;
	private readonly Context _dbContext;
	private readonly UserManager<User> _userManager;
	private readonly RoleManager<IdentityUserRole> _roleManager;
	private readonly ITokenProvider _tokenProvider;
	private readonly IGoogleAuthentication _googleAuthentication;

	public UserRepository(ILogger<UserRepository> logger, Context dbContext, UserManager<User> userManager, 
		RoleManager<IdentityUserRole> roleManager, ITokenProvider tokenProvider, IGoogleAuthentication googleAuthentication)
	{
		_dbContext = dbContext;
		_userManager = userManager;
		_roleManager = roleManager;
		_tokenProvider = tokenProvider;
		_googleAuthentication = googleAuthentication;
		_logger = logger;
	}

	public async Task<Result<User>> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Find user with id: {id}", id);
		
		var user = await _dbContext.Users.FindAsync(new object?[] { id }, cancellationToken);
		return user is null ? Error.NotFound(UserResources.UserDoesNotExist) : user;
	}

	public async Task<Result<User>> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Find user with email: {email}", email);
		
		var user = await _userManager.FindByEmailAsync(email);
		return user is null ? Error.NotFound(UserResources.UserDoesNotExist) : user;
	}

	public async Task<Result<string>> GenerateEmailConfirmationTokenAsync(User user, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Generate email confirmation token for user with id: {id}", user.Id);
		
		var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
		return token;
	}
	
	public async Task<Result<string>> RegisterAsync(User user, string? password, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Register user with email: {email}", user.Email);

		IdentityResult result;
		if (password is null)
		{
			result = await _userManager.CreateAsync(user);
		}
		else
		{
			result = await _userManager.CreateAsync(user, password);
		}
		if (!result.Succeeded)
		{
			Helper.LogError(_logger, result);
			return Error.Invalid(result.Errors.Select(e => e.Description).ToList());
		}
		
		result = await _userManager.AddToRoleAsync(user, UserRole.User.ToString());
		if (!result.Succeeded)
		{
			Helper.LogError(_logger, result);
			return Error.Invalid(UserResources.FailedToRegisterUser);
		}

		var claims = CreateClaims(user);
		result = await _userManager.AddClaimsAsync(user, claims);
		if (!result.Succeeded)
		{
			Helper.LogError(_logger, result);
			return Error.Invalid(UserResources.FailedToRegisterUser);
		}

		return await GenerateEmailConfirmationTokenAsync(user, cancellationToken);
	}

	public async Task<Result> ConfirmEmailAsync(User user, string token, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Confirm email for user with id: {id}", user.Id);
		
		var result = await _userManager.ConfirmEmailAsync(user, token);
		if (!result.Succeeded)
		{
			Helper.LogError(_logger, result);
			return Error.Invalid(UserResources.FailedToRegisterUser);
		}

		return Result.Ok();
	}

	public async Task<Result> UpdateAsync(User user, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Update user with id: {id}", user.Id);
		
		_dbContext.Users.Update(user);
		return await Task.FromResult(Result.Ok());
	}
	
	public async Task<Result<TokensDto>> LoginAsync(User user, string password, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Login user with id: {id}", user.Id);

		if (!await _userManager.IsEmailConfirmedAsync(user).ConfigureAwait(false))
		{
			return Error.Unauthorized(UserResources.EmailIsNotConfirmed);
		}

		if (!await _userManager.CheckPasswordAsync(user, password).ConfigureAwait(false))
		{
			return Error.Unauthorized(UserResources.InvalidPasswordOrEmail);
		}
		
		var authClaims = await GetClaims(user);
		if (authClaims.IsUnsuccessful)
		{
			return authClaims.Error;
		}
		
		await RemoveTokens(user, cancellationToken);
		
		var accessToken = _tokenProvider.GenerateAccessToken(authClaims.Value);
		var refreshToken = _tokenProvider.GenerateRefreshToken();

		await _userManager.SetAuthenticationTokenAsync(user, GrantType.Password.ToStringFast(), "RefreshToken", refreshToken);

		return new TokensDto(accessToken, refreshToken);
	}
	
	private static readonly Func<Context, string, Task<IdentityUserToken<Guid>?>> getUserTokenByRefreshTokenAsync = 
		EF.CompileAsyncQuery((Context context, string refreshToken) =>
			context.UserTokens.FirstOrDefault(t => t.Value == refreshToken));
	
	public async Task<Result<User>> FindByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
	{
		var userToken = await getUserTokenByRefreshTokenAsync(_dbContext, refreshToken);
		if (userToken is null)
		{
			return Error.NotFound(UserResources.UserDoesNotExist);
		}
		
		return await FindByIdAsync(userToken.UserId, cancellationToken);
	}
	
	public async Task<Result<TokensDto>> LoginWithRefreshTokenAsync(User user, string refreshToken, CancellationToken cancellationToken = default)
	{
		foreach(var authenticator in GrantTypeExtensions.GetNames())
		{
			var userRefreshToken = await _userManager.GetAuthenticationTokenAsync(user, authenticator, "RefreshToken");
			if (userRefreshToken is null)
			{
				continue;
			}
			
			if (userRefreshToken != refreshToken)
			{
				await _userManager.RemoveAuthenticationTokenAsync(user, authenticator, "RefreshToken");
				return Error.Unauthorized(UserResources.InvalidRefreshToken);
			}
			
			await RemoveTokens(user, cancellationToken);

			var authClaims = await GetClaims(user);
			if (authClaims.IsUnsuccessful)
			{
				return authClaims.Error;
			}
			
			var accessToken = _tokenProvider.GenerateAccessToken(authClaims.Value);
			var newRefreshToken = _tokenProvider.GenerateRefreshToken();

			await _userManager.RemoveAuthenticationTokenAsync(user, authenticator, "RefreshToken");
			await _userManager.SetAuthenticationTokenAsync(user, GrantType.RefreshToken.ToStringFast(), "RefreshToken", newRefreshToken);

			return new TokensDto(accessToken, newRefreshToken);
		}
		
		return Error.Unauthorized(UserResources.FailedToLoginWithRefreshToken);
	}
	
	public async Task<Result<TokensDto>> LoginWithGoogleAsync(string idToken, CancellationToken cancellationToken = default)
	{
		var googleUser = await _googleAuthentication.AuthenticateAsync(idToken);
		if (googleUser.IsUnsuccessful)
		{
			_logger.LogError("Database - Failed to authenticate via Google");
			return googleUser.Error;
		}
		
		if (!googleUser.Value.IsRegistered)
		{
			var newUser = _googleAuthentication.GetUser(googleUser.Value.Payload);
			
			var token = await RegisterAsync(newUser, null, cancellationToken);
			if (token.IsUnsuccessful)
			{
				_logger.LogError("Database - Failed to register user");
				return token.Error;
			}

			if (_googleAuthentication.IsEmailVerified(googleUser.Value.Payload))
			{
				await _userManager.ConfirmEmailAsync(newUser, token.Value);
			}
		}
		
		var userResult = await FindByEmailAsync(googleUser.Value.Payload.Email, cancellationToken);
		if (userResult.IsUnsuccessful)
		{
			_logger.LogError("Database - Failed to find user");
			return userResult.Error;
		}
		var user = userResult.Value;
		
		var authClaims = await GetClaims(user);
		if (authClaims.IsUnsuccessful)
		{
			return authClaims.Error;
		}
		
		await RemoveTokens(user, cancellationToken);
		
		var accessToken = _tokenProvider.GenerateAccessToken(authClaims.Value);
		var refreshToken = _tokenProvider.GenerateRefreshToken();

		await _userManager.SetAuthenticationTokenAsync(user, GrantType.Google.ToStringFast(), "RefreshToken", refreshToken);

		return new TokensDto(accessToken, refreshToken);
	}

	public async Task<Result> LogoutAsync(User user, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Logout user with id: {id}", user.Id);

		return await RemoveTokens(user, cancellationToken);
	}

	public async Task<Result<Information>> GetInformationAsync(Guid id, CancellationToken cancellationToken = default)
	{
		var user = await FindByIdAsync(id, cancellationToken);
		if (user.IsUnsuccessful)
		{
			return user.Error;
		}
		
		return user.Value.Information;
	}

	public async Task<Result> AddPersonalityAsync(Guid id, PersonId personId, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Add personality with id: {personId} to user with id: {id}", personId, id);
		
		var user = await FindByIdAsync(id, cancellationToken);
		if(user.IsUnsuccessful)
		{
			return user.Error;
		}
		if(user.Value.PersonId is not null)
		{
			return Error.Invalid(UserResources.UserAlreadyHavePersonality);
		}
		
		await _userManager.AddClaimAsync(user.Value, new Claim(ClaimNames.personId, personId.ToString()));
		
		user.Value.PersonId = personId;
		
		return Result.Ok();
	}

	public async Task<Result<MediaInfo?>> FindMediaInfoAsync(Guid id, MediaId mediaId, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Find media info for user with id: {id}", id);

		var user = await FindByIdAsync(id, cancellationToken);
		if (user.IsUnsuccessful)
		{
			return user.Error;
		}
		
		var mediaInfo = user.Value.MediaInfos.FirstOrDefault(m => m.MediaId == mediaId);
		return mediaInfo;
	}

	private async Task<Result<IEnumerable<string>>> GetRolesAsync(User user, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Get roles for user with id: {id}", user.Id);
		
		var roles = await _userManager.GetRolesAsync(user);
		return roles.Count != 0 ? Result<IEnumerable<string>>.Ok(roles) : Error.InternalError(UserResources.FatalError);
	}
	
	public async Task<Result> UpdateRolesAsync(User user, IList<string> roles, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Update roles for user with id: {id}", user.Id);

		var currentRoles = await GetRolesAsync(user, cancellationToken);
		if (currentRoles.IsUnsuccessful)
		{
			return currentRoles.Error;
		}
		var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles.Value);
		if (!removeResult.Succeeded)
		{
			return Error.InternalError(UserResources.FatalError);
		}

		if (!roles.Contains("User"))
		{
			roles.Add("User");
		}
		
		var result = await _userManager.AddToRolesAsync(user, roles);
		return result.Succeeded ? Result.Ok() : Error.Invalid(UserResources.FailedToAddRoles);
	}
	
	public async Task<Result> AddRoleAsync(User user, string role, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Add role for user with id: {id}", user.Id);
		
		var result = await _userManager.AddToRoleAsync(user, role);
		return result.Succeeded ? Result.Ok() : Error.Invalid(UserResources.FailedToAddRoles);
	}
	
	public async Task<Result> RemoveRoleAsync(User user, string role, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Remove role for user with id: {id}", user.Id);
		
		var result = await _userManager.RemoveFromRoleAsync(user, role);
		return result.Succeeded ? Result.Ok() : Error.Invalid(UserResources.FailedToAddRoles);
	}

	public async Task<bool> IsSystemAdministratorAsync(User user, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Check if user with id: {id} is system administrator", user.Id.ToString());
		
		return await _userManager.IsInRoleAsync(user, UserRole.SystemAdministrator.ToStringFast());
	}

	public async Task<Result> ChangePasswordAsync(User user, string currentPassword, string newPassword, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Change password for user with id: {id}", user.Id);

		if (user.PasswordHash is null)
		{
			return Error.Invalid(UserResources.CanNotChangeExternalLoginPassword);
		}
		
		var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
		return result.Succeeded ? Result.Ok() : Error.Invalid(UserResources.FailedToChangePassword);
	}

	public async Task<Result> ChangeUsernameAsync(User user, string username, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Change username for user with id: {id}", user.Id);

		var result = await _userManager.SetUserNameAsync(user, username);
		return result.Succeeded ? Result.Ok() : Error.Invalid(UserResources.FailedToChangeUsername);
	}
	
	public async Task<Result> ChangeEmailAsync(User user, string email, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Change username for user with id: {id}", user.Id);

		var result = await _userManager.SetEmailAsync(user, email);
		return result.Succeeded ? Result.Ok() : Error.Invalid(UserResources.FailedToChangeEmail);
	}


	private async Task<Result> RemoveTokens(User user, CancellationToken cancellationToken = default)
	{
		foreach (var authenticator in GrantTypeExtensions.GetNames())
		{
			await _userManager.RemoveAuthenticationTokenAsync(user, authenticator, "RefreshToken");
		}

		return Result.Ok();
	}
	
	private static IEnumerable<Claim> CreateClaims(User user)
	{
		var claims = new List<Claim>
		{
			new(ClaimNames.id, user.Id.ToString()),
			new(ClaimNames.email, user.Email!),
			new(ClaimNames.displayName, string.IsNullOrWhiteSpace(user.Information.FirstName) ? user.UserName! : user.Information.FirstName),
			new(ClaimNames.age, user.Information.Age.ToString()),
		};
		
		return claims;
	} 
	
	private async Task<Result<List<Claim>>> GetClaims(User user)
	{
		var authClaims = (await _userManager.GetClaimsAsync(user).ConfigureAwait(false)).ToList();

		foreach (var userRole in await _userManager.GetRolesAsync(user).ConfigureAwait(false))
		{
			var role = await _roleManager.FindByNameAsync(userRole);
			if (role is null)
			{
				_logger.LogError("Database - Role {role} does not exist", userRole);
				return Error.Invalid(UserResources.CloudNotSignIn);
			}
			
			var claims = await _roleManager.GetClaimsAsync(role);
			authClaims.AddRange(claims);
		}

		return authClaims;
	}
}