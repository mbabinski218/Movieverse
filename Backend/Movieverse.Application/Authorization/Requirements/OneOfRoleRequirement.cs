using Microsoft.AspNetCore.Authorization;
using Movieverse.Domain.Common.Types;

namespace Movieverse.Application.Authorization.Requirements;

public sealed class OneOfRoleRequirement : IAuthorizationRequirement
{
	public UserRole[] Roles { get; }

	public OneOfRoleRequirement(params UserRole[] roles)
	{
		Roles = roles;
	}
}