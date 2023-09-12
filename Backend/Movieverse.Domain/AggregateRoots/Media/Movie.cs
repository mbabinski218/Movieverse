using Movieverse.Domain.Entities;
using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.Domain.AggregateRoots.Media;

public sealed class Movie : Media
{
	// Map to table
	public AggregateRootId? SequelId { get; set; }
	public string? SequelTitle { get; set; }
	public AggregateRootId? PrequelId { get; set; }
	public string? PrequelTitle { get; set; }
	
	// EF Core
	private Movie()
	{
		
	}
	
	private Movie(AggregateRootId id, string title) : base(id, title)
	{
		
	}
	
	// Other
	public static Movie Create(AggregateRootId id, string title)
	{
		var movie = new Movie(id, title);
		movie.AdvancedStatistics = Statistics.Create(movie);
		return movie;
	}
	public static Movie Create(string title) => Create(AggregateRootId.Create(), title);
}