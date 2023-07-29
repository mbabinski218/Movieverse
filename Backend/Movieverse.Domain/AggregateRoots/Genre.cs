using Movieverse.Domain.Common.Models;
using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.Domain.AggregateRoots;

public class Genre : AggregateRoot
{
	// Map to table
	public string Name { get; set; } = null!;
	public string Description { get; set; } = null!;
	public uint MediaCount { get; set; }
	public virtual List<AggregateRootId> MediaIds { get; private set; } = new();

	// EF Core
	private Genre()
	{
		
	}
	
	// Other
	private Genre(Guid id, string name, string description) : base(id)
	{
		Name = name;
		Description = description;
	}

	public static Genre Create(string name, string description)
		=> new(Guid.NewGuid(), name, description);
}