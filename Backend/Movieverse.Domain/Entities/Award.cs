using Movieverse.Domain.Common.Models;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Domain.Entities;

public class Award : Entity<int>
{
	// Map to table
	public string Name { get; set; } = null!;
	public string? Description { get; set; }
	public ContentId? ImageId { get; set; } = null!;
	public virtual List<StatisticsAward> StatisticsAwards { get; private set; } = new();

	// EF Core
	private Award()
	{
		
	}
	
	// Other
}