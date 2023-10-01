using Microsoft.AspNetCore.Identity;

namespace Movieverse.Domain.Common.Models;

public sealed class IdentityUserRole : IdentityRole<Guid>
{
	public IdentityUserRole(string name) : base(name)
	{
	}
}