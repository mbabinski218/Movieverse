using Microsoft.AspNetCore.Authorization;
using Movieverse.Application.Authorization.Requirements;
using Movieverse.Application.Interfaces;
using Movieverse.Domain.Common.Types;

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
		var userRole = _httpService.Role;
		if (userRole is null)
		{
			return Task.CompletedTask;
		}
		
		if (requirement.Roles.Contains((UserRole)userRole))
		{
			context.Succeed(requirement);
		}
        
		return Task.CompletedTask;
	}
}