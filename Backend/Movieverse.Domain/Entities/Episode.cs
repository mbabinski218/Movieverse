using Movieverse.Domain.Common.Models;
using Movieverse.Domain.ValueObjects;

namespace Movieverse.Domain.Entities;

public class Episode : Entity<int>
{
	// Map to table
	public virtual Season Season { get; private set; } = null!;
	public short EpisodeNumber { get; set; }
	public string Title { get; set; } = null!;
	public Details Details { get; set; } = null!;
	
	// Constructors
	private Episode(Season season, short episodeNumber, string title, Details details)
	{
		Season = season;
		EpisodeNumber = episodeNumber;
		Title = title;
		Details = details;
	}
	
	// Methods
	public static Episode Create(Season season, short episodeNumber, string title, Details details)
		=> new (season, episodeNumber, title, details);
		
	// EF Core
	private Episode()
	{
			
	}
}