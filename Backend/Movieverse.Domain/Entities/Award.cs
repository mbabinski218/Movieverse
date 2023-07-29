using Movieverse.Domain.Common.Models;
using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.Domain.Entities;

public class Award : Entity
{
	// Map to table
	public string Name { get; set; } = null!;
	public string? Description { get; set; }
	public AggregateRootId? ImageId { get; set; } = null!;
	public virtual List<StatisticsAward> StatisticsAwards { get; private set; } = new();

	// EF Core
	private Award()
	{
		
	}
	
	// Other
}