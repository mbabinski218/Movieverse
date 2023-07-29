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
	
	// Other
}