using Movieverse.Domain.Common.Models;
using Movieverse.Domain.ValueObjects;
using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.Domain.Entities;

public class Episode : Entity
{
	// Map to table
	public virtual Season Season { get; private set; } = null!;
	public short EpisodeNumber { get; set; }
	public string Title { get; set; } = null!;
	public Details Details { get; set; } = null!;
	public BasicStatistics BasicStatistics { get; set; } = null!;
	public virtual List<AggregateRootId> ContentIds { get; private set; } = new();
	
	// EF Core
	private Episode()
	{
			
	}
	
	private Episode(Season season, short episodeNumber, string title, Details details)
	{
		Season = season;
		EpisodeNumber = episodeNumber;
		Title = title;
		Details = details;
		BasicStatistics = new BasicStatistics();
	}
	
	// Other
	public static Episode Create(Season season, short episodeNumber, string title, Details details)
		=> new (season, episodeNumber, title, details);
}