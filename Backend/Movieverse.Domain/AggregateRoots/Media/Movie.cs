using Movieverse.Domain.Entities;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Domain.AggregateRoots.Media;

public sealed class Movie : Media
{
	// Map to table
	public MediaId? SequelId { get; set; }
	public string? SequelTitle { get; set; }
	public MediaId? PrequelId { get; set; }
	public string? PrequelTitle { get; set; }
	
	// Constructors
	private Movie(MediaId id, string title) : base(id, title)
	{
		
	}

	// Methods
	public static Movie Create(MediaId id, string title)
	{
		var movie = new Movie(id, title);
		movie.AdvancedStatistics = Statistics.Create(movie);
		return movie;
	}
	
	public static Movie Create(string title) => Create(MediaId.Create(), title);
	
	// EF Core
	private Movie()
	{
		
	}
}