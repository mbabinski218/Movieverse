using Movieverse.Domain.Entities;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Domain.AggregateRoots.Media;

public class Series : Media
{
	// Map to table
	public virtual List<Season> Seasons { get; private set; } = new();
	public short SeasonCount { get; set; }
	public short EpisodeCount { get; set; }
	public DateTimeOffset? SeriesEnded { get; set; }

	// EF Core
	private Series()
	{
		
	}
	
	private Series(MediaId id, string title, short? seasonCount) : base(id, title)
	{
		SeasonCount = seasonCount ?? 0;
	}
	
	// Other
	public static Series Create(MediaId id, string title, short? seasonCount = null)
	{
		var series = new Series(id, title, seasonCount);
		series.AdvancedStatistics = Statistics.Create(series);
		return series;
	}
	public static Series Create(string title, short? seasonCount = null) => new(MediaId.Create(), title, seasonCount);
}