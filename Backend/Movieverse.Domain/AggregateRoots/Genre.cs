using Movieverse.Domain.Common.Models;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Domain.AggregateRoots;

public class Genre : AggregateRoot<GenreId, Guid>
{
	// Map to table
	public string Name { get; set; } = null!;
	public string Description { get; set; } = null!;
	public int MediaCount { get; set; }
	public virtual List<MediaId> MediaIds { get; private set; } = new();

	// EF Core
	private Genre()
	{
		
	}
	
	// Other
	private Genre(GenreId id, string name, string description) : base(id)
	{
		Name = name;
		Description = description;
		MediaCount = 0;
	}

	public static Genre Create(string name, string description)
		=> new(GenreId.Create(Guid.NewGuid()), name, description);
}