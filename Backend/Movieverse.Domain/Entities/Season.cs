using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Common.Models;

namespace Movieverse.Domain.Entities;

public class Season : Entity
{
	// Map to table
	public virtual Series Series { get; set; } = null!;
	public short SeasonNumber { get; set; }
	public virtual List<Episode> Episodes { get; private set; } = new();
	public short EpisodeCount { get; set; }

	// EF Core
	private Season()
	{
		
	}
	
	// Other
	public static Season Create(Series series, int seasonNumber, int episodeCount = 0) =>
		Create(series, (short)seasonNumber, (short)episodeCount);
	
	public static Season Create(Series series, short seasonNumber, short episodeCount = 0)
	{
		return new Season
		{
			Series = series,
			SeasonNumber = seasonNumber,
			EpisodeCount = episodeCount
		};
	}
}