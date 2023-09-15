using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Common.Models;
using Movieverse.Domain.Common.Types;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Domain.Entities;

public class Staff : Entity<int>
{
	// Map to table
	public virtual Media Media { get; private set; } = null!;
	public PersonId PersonId { get; private set; } = null!;
	public Role Role { get; set; }

	// EF Core
	private Staff()
	{
		
	}
	
	// Other
	public static Staff Create(Media media, PersonId personId, Role role)
	{
		return new Staff
		{
			Media = media,
			PersonId = personId,
			Role = role
		};
	}
}