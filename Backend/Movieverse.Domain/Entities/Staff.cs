using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Common.Models;
using Movieverse.Domain.Common.Types;
using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.Domain.Entities;

public class Staff : Entity
{
	// Map to table
	public virtual Media Media { get; private set; } = null!;
	public AggregateRootId PersonId { get; private set; } = null!;
	public Role Role { get; set; }

	// EF Core
	private Staff()
	{
		
	}
	
	// Other
	public static Staff Create(Media media, AggregateRootId personId, Role role)
	{
		return new Staff
		{
			Media = media,
			PersonId = personId,
			Role = role
		};
	}
}