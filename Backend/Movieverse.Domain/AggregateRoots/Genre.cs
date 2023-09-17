using Movieverse.Domain.Common.Models;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Domain.AggregateRoots;

public sealed class Genre : AggregateRoot<GenreId, Guid>
{
	// Map to table
	private readonly List<MediaId> _mediaIds = new();
	
	public string Name { get; set; } = null!;
	public string Description { get; set; } = null!;
	public int MediaCount { get; set; }
	public IReadOnlyList<MediaId> MediaIds => _mediaIds.AsReadOnly();
	
	// Constructors
	private Genre(GenreId id, string name, string description) : base(id)
	{
		Name = name;
		Description = description;
		MediaCount = 0;
	}

	// Methods
	public static Genre Create(string name, string description) => new(GenreId.Create(Guid.NewGuid()), name, description);
	
	public void AddMedia(MediaId mediaId)
	{
		_mediaIds.Add(mediaId);
		MediaCount++;
	}

	// Equality
	public override bool Equals(object? obj) => obj is GenreId entityId && Id.Equals(entityId);
	
	public override int GetHashCode() => Id.GetHashCode();
	
	// EF Core
	private Genre()
	{
		
	}
}