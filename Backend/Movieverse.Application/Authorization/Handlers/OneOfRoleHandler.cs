using Microsoft.AspNetCore.Authorization;
using Movieverse.Application.Authorization.Requirements;
using Movieverse.Application.Interfaces;

namespace Movieverse.Application.Authorization.Handlers;

public sealed class OneOfRoleHandler : AuthorizationHandler<OneOfRoleRequirement>
{
	private readonly IHttpService _httpService;

	public OneOfRoleHandler(IHttpService httpService)
	{
		_httpService = httpService;
	}

	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OneOfRoleRequirement requirement)
	{
		var userRoles = _httpService.Role;
		if (userRoles is null)
		{
			return Task.CompletedTask;
		}

		foreach (var userRole in userRoles)
		{
			if (requirement.Roles.Contains(userRole))
			{
				context.Succeed(requirement);
			}
		}

		return Task.CompletedTask;
	}
}