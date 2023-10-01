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
	
	//Constructors
	private Staff(Media media, PersonId personId, Role role)
	{
		Media = media;
		PersonId = personId;
		Role = role;
	}
	
	// Methods
	public static Staff Create(Media media, PersonId personId, Role role) => new(media, personId, role);
	
	// EF Core
	private Staff()
	{
		
	}
}