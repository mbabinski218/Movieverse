using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Common.Models;
using Movieverse.Domain.ValueObjects;

namespace Movieverse.Domain.Entities;

public class Statistics : Entity<int>
{
	// Map to table
	private readonly List<Popularity> _popularity = new();
	
	public virtual Media Media { get; private set; } = null!;
	public BoxOffice BoxOffice { get; set; } = null!;
	public IReadOnlyList<Popularity> Popularity => _popularity.AsReadOnly();
	
	// Constructors
	private Statistics(Media media)
	{
		Media = media;
		BoxOffice = new BoxOffice();
	}
	
	// Methods
	public static Statistics Create(Media media)
	{
		var statistics = new Statistics(media);
		statistics.AddPopularity(Entities.Popularity.Create(statistics, DateTimeOffset.UtcNow));
		return statistics;
	}
	
	public void AddPopularity(Popularity popularity)
	{
		_popularity.Add(popularity);
	}
	
	// EF Core
	private Statistics()
	{
		
	}
}