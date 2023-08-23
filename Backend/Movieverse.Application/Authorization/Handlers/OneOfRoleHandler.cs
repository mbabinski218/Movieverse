using Microsoft.AspNetCore.Authorization;
using Movieverse.Application.Authorization.Requirements;
using Movieverse.Domain.Common.Types;

namespace Movieverse.Application.Authorization.Handlers;

public sealed class OneOfRoleHandler : AuthorizationHandler<OneOfRoleRequirement>
{
	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OneOfRoleRequirement requirement)
	{
		if (requirement.Roles.Any(role => context.User.IsInRole(role.ToStringFast())))
		{
			context.Succeed(requirement);
		}
        
		return Task.CompletedTask;
	}
}