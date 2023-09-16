using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Models;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Domain.Entities;

public class MediaInfo : Entity<int>
{
	// Map to table
	public virtual User User { get; private set; } = null!;
	public MediaId MediaId { get; private set; } = null!;
	public bool IsInWatchlist { get; set; }
	public ushort Rating { get; set; }

	// EF Core
	private MediaInfo()
	{
			
	}
}
