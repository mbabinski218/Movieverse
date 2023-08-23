using Microsoft.AspNetCore.Authorization;
using Movieverse.Application.Authorization.Requirements;
using Movieverse.Domain.Common.Types;

namespace Movieverse.Application.Authorization.Handlers;

public sealed class RoleHandler : AuthorizationHandler<RoleRequirement>
{
	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
	{
		if (context.User.IsInRole(requirement.Role.ToStringFast()))
		{
			context.Succeed(requirement);
		}

		return Task.CompletedTask;
	}
}