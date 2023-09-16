using Movieverse.Domain.Common.Models;
using Movieverse.Domain.ValueObjects;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Domain.Entities;

public class Episode : Entity<int>
{
	// Map to table
	private readonly List<ContentId> _contentIds = new();
	
	public virtual Season Season { get; private set; } = null!;
	public short EpisodeNumber { get; set; }
	public string Title { get; set; } = null!;
	public Details Details { get; set; } = null!;
	public BasicStatistics BasicStatistics { get; set; } = null!;
	public IReadOnlyList<ContentId> ContentIds => _contentIds.AsReadOnly();
	
	// Constructors
	private Episode(Season season, short episodeNumber, string title, Details details)
	{
		Season = season;
		EpisodeNumber = episodeNumber;
		Title = title;
		Details = details;
		BasicStatistics = new BasicStatistics();
	}
	
	// Methods
	public static Episode Create(Season season, short episodeNumber, string title, Details details)
		=> new (season, episodeNumber, title, details);
	
	public void AddContent(ContentId contentId)
	{
		_contentIds.Add(contentId);
	}
	
	public void RemoveContent(ContentId contentId)
	{
		_contentIds.Remove(contentId);
	}
		
	// EF Core
	private Episode()
	{
			
	}
}