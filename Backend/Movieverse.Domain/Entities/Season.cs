using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Common.Models;

namespace Movieverse.Domain.Entities;

public class Season : Entity<int>
{
	// Map to table
	private readonly List<Episode> _episodes = new();
	
	public virtual Series Series { get; set; } = null!;
	public short SeasonNumber { get; set; }
	public IReadOnlyList<Episode> Episodes => _episodes.AsReadOnly();
	public short EpisodeCount { get; set; }
	
	// Constructors
	public Season(Series series, short seasonNumber, short episodeCount = 0)
	{
		Series = series;
		SeasonNumber = seasonNumber;
		EpisodeCount = episodeCount;
	}
	
	// Methods
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
	
	public void AddEpisode(Episode episode)
	{
		_episodes.Add(episode);
		EpisodeCount++;
	}
	
	public void RemoveEpisode(Episode episode)
	{
		_episodes.Remove(episode);
		EpisodeCount--;
	}
	
	// EF Core
	private Season()
	{
		
	}
}