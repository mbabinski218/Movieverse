using Microsoft.AspNetCore.Authorization;
using Movieverse.Domain.Common.Types;

namespace Movieverse.Application.Authorization.Requirements;

public sealed class RoleRequirement : IAuthorizationRequirement
{
	public UserRole Role { get; }

	public RoleRequirement(UserRole role)
	{
		Role = role;
	}
}