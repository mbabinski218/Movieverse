using Movieverse.Domain.Entities;

namespace Movieverse.Domain.AggregateRoots;

public class Series : Media
{
	public virtual List<Season> Seasons { get; set; } = new();
	public int? SeasonCount { get; set; }
	public int? EpisodeCount { get; set; }
}