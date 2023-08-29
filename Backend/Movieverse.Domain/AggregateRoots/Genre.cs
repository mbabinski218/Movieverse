using Movieverse.Domain.Common.Models;
using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.Domain.AggregateRoots;

public class Genre : AggregateRoot
{
	// Map to table
	public string Name { get; set; } = null!;
	public string Description { get; set; } = null!;
	public int MediaCount { get; set; }
	public virtual List<AggregateRootId> MediaIds { get; private set; } = new();

	// EF Core
	private Genre()
	{
		
	}
	
	// Other
	private Genre(AggregateRootId id, string name, string description) : base(id)
	{
		Name = name;
		Description = description;
		MediaCount = 0;
	}

	public static Genre Create(string name, string description)
		=> new(AggregateRootId.Create(Guid.NewGuid()), name, description);
}