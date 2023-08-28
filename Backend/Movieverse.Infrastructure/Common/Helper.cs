using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Resources;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Models;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Infrastructure.Common;

public static class Helper
{
	public static void LogError(ILogger logger, IdentityResult? result)
	{
		var errors = result?.Errors.Select(e => e.Description).ToList() ?? new List<string>();
		logger.LogError("Msg: {errors}",string.Join(", ", errors));
	}
	
	public static async Task<Result<List<Claim>>> GetUserClaims(ILogger logger, UserManager<User> userManager, 
		RoleManager<IdentityUserRole> roleManager, User user)
	{
		var authClaims = (await userManager.GetClaimsAsync(user).ConfigureAwait(false)).ToList();

		foreach (var userRole in await userManager.GetRolesAsync(user).ConfigureAwait(false))
		{
			var role = await roleManager.FindByNameAsync(userRole).ConfigureAwait(false);

			if (role is null)
			{
				logger.LogError("Role {role} does not exist", userRole);
				return Error.Invalid(UserResources.CloudNotSignIn);
			}
			
			var claims = await roleManager.GetClaimsAsync(role).ConfigureAwait(false);
			authClaims.AddRange(claims);
		}

		return authClaims;
	}
}