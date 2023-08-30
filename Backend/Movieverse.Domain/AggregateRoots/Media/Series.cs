using Movieverse.Domain.Entities;
using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.Domain.AggregateRoots.Media;

public class Series : Media
{
	// Map to table
	public virtual List<Season> Seasons { get; private set; } = new();
	public short SeasonCount { get; set; }
	public short EpisodeCount { get; set; }

	// EF Core
	private Series()
	{
		
	}
	
	private Series(AggregateRootId id, string title, short? seasonCount) : base(id, title)
	{
		SeasonCount = seasonCount ?? 0;
	}
	
	// Other
	public static Series Create(AggregateRootId id, string title, short? seasonCount = null) => new(id, title, seasonCount);
	public static Series Create(string title, short? seasonCount = null) => new(AggregateRootId.Create(), title, seasonCount);
}