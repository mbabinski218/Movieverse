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
	public virtual List<Review> Reviews { get; private set; } = new();
	
	// EF Core
	private Episode()
	{
			
	}
	
	// Other
}