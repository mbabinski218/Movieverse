using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Common.Result;
using Movieverse.Application.Interfaces;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Models;
using Movieverse.Domain.Common.Types;

namespace Movieverse.Infrastructure.Repositories;

public sealed class UserRepository : IUserRepository
{
	private readonly ILogger<UserRepository> _logger;
	private readonly UserManager<User> _userManager;
	private readonly RoleManager<IdentityUserRole> _roleManager;

	public UserRepository( ILogger<UserRepository> logger, UserManager<User> userManager, RoleManager<IdentityUserRole> roleManager)
	{
		_logger = logger;
		_userManager = userManager;
		_roleManager = roleManager;
	}

	public async Task<Result> RegisterAsync(User user, string password)
	{
		var result = await _userManager.CreateAsync(user, password);
		
		if(!result.Succeeded) GenerateError(user.Email, result);
		
		result = await _userManager.AddToRoleAsync(user, UserRole.User.ToString());
        
		if(!result.Succeeded) GenerateError(user.Email, result);

		var claims = new List<Claim>
		{
			new(ClaimNames.id, user.Id.ToString()),
			new(ClaimNames.email, user.Email!),
			new(ClaimNames.displayName, user.Information.FirstName ?? user.UserName!),
			new(ClaimNames.age, user.Information.Age.ToString()),
			new(ClaimNames.role, UserRole.User.ToString())
		};
		result = await _userManager.AddClaimsAsync(user, claims);

		return result.Succeeded ? Result.Ok() : GenerateError(user.Email, result);
	}

	private Error GenerateError(string? userEmail, IdentityResult? result)
	{
		_logger.LogDebug("Failed to register user {email}, error: {error}.", userEmail, result?.Errors.ToString());
		
		var errors = result?.Errors.Select(e => e.Description).ToList() ?? new List<string>();
		
		return Error.Invalid(errors);
	}
}