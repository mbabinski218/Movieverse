using Movieverse.Domain.Common.Models;
using Movieverse.Domain.Entities;
using Movieverse.Domain.ValueObjects;
using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.Domain.AggregateRoots;

public class User : IdentityAggregateRoot
{
	// Map to table
	public Information Information { get; set; } = null!;
	public AggregateRootId? AvatarId { get; set; } = null!;
	public virtual List<MediaInfo> MediaInfos { get; private set; } = new();
	public AggregateRootId? PersonId { get; set; }

	// EF Core
	private User()
	{
		
	}
	
	// Other
}