using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Models;
using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.Domain.Entities;

public class MediaInfo : Entity
{
	// Map to table
	public virtual User User { get; private set; } = null!;
	public AggregateRootId MediaId { get; private set; } = null!;
	public bool IsInWatchlist { get; set; }
	public ushort Rating { get; set; }

	// EF Core
	private MediaInfo()
	{
			
	}
	
	// Other
}