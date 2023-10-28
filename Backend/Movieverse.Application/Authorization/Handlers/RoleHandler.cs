using Microsoft.AspNetCore.Authorization;
using Movieverse.Application.Authorization.Requirements;
using Movieverse.Application.Interfaces;

namespace Movieverse.Application.Authorization.Handlers;

public sealed class RoleHandler : AuthorizationHandler<RoleRequirement>
{
	private readonly IHttpService _httpService;

	public RoleHandler(IHttpService httpService)
	{
		_httpService = httpService;
	}
	
	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
	{
		var userRole = _httpService.Role;
		if (userRole is null)
		{
			return Task.CompletedTask;
		}
		
		if (userRole.Contains(requirement.Role))
		{
			context.Succeed(requirement);
		}

		return Task.CompletedTask;
	}
}