using Movieverse.Domain.Common.Models;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Domain.AggregateRoots;

public sealed class Content : AggregateRoot<ContentId, Guid>
{
	// Map to table
	public string Path { get; set; } = null!;
	public string ContentType { get; set; } = null!;
	public string? Title { get; set; }
	
	// Constructors
	private Content(ContentId id, string path, string contentType, string? title) : base(id)
	{
		Path = path;
		ContentType = contentType;
		Title = title;
	}

	// Methods
	public static Content Create(ContentId id, string path, string contentType, string? title = null) => 
		new(id, path, contentType, title);
	
	// Equality
	public override bool Equals(object? obj) => obj is ContentId entityId && Id.Equals(entityId);

	public override int GetHashCode() => Id.GetHashCode();
	
	// EF Core
	private Content()
	{
		
	}
}