using Movieverse.Domain.Common.Models;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Domain.AggregateRoots;

public class Content : AggregateRoot<ContentId, Guid>
{
	// Map to table
	public string Path { get; set; } = null!;
	public string ContentType { get; set; } = null!;
	public string? Title { get; set; }

	// EF Core
	private Content()
	{
		
	}
	
	// Methods
	private Content(ContentId id, string path, string contentType, string? title) : base(id)
	{
		Path = path;
		ContentType = contentType;
		Title = title;
	}

	public static Content Create(ContentId id, string path, string contentType, string? title = null) => 
		new(id, path, contentType, title);
}