using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Common.Models;

namespace Movieverse.Domain.Entities;

public class Season : Entity
{
	// Map to table
	public virtual Series Series { get; set; } = null!;
	public uint SeasonNumber { get; set; }
	public virtual List<Episode> Episodes { get; private set; } = new();
	public uint? EpisodeCount { get; set; }

	// EF Core
	private Season()
	{
		
	}
	
	// Other
	public static Season Create(Series series, short seasonNumber, int episodeCount)
	{
		return new Season
		{
			Series = series,
			SeasonNumber = (uint)seasonNumber,
			EpisodeCount = (uint)episodeCount
		};
	}
}