using Movieverse.Domain.Entities;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Domain.AggregateRoots.Media;

public sealed class Series : Media
{
	// Map to table
	private readonly List<Season> _seasons = new();
	
	public IReadOnlyList<Season> Seasons => _seasons.AsReadOnly();
	public short SeasonCount { get; set; }
	public short EpisodeCount { get; set; }
	public DateTimeOffset? SeriesEnded { get; set; }
	
	// Constructors
	private Series(MediaId id, string title, short? seasonCount) : base(id, title)
	{
		SeasonCount = seasonCount ?? 0;
	}
	
	// Methods
	public static Series Create(MediaId id, string title, short? seasonCount = null)
	{
		var series = new Series(id, title, seasonCount);
		series.AdvancedStatistics = Statistics.Create(series);
		return series;
	}
	public static Series Create(string title, short? seasonCount = null) => new(MediaId.Create(), title, seasonCount);
	
	public void AddSeason(Season season)
	{
		_seasons.Add(season);
		SeasonCount++;
	}
	
	public void RemoveSeason(Season season)
	{
		_seasons.Remove(season);
		SeasonCount--;
	}
	
	// EF Core
	private Series()
	{
		
	}
}