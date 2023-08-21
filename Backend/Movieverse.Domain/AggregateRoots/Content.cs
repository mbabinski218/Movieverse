using Movieverse.Domain.Common.Models;
using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.Domain.AggregateRoots;

public class Content : AggregateRoot
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
	private Content(AggregateRootId id, string path, string contentType, string? title) : base(id)
	{
		Id = id;
		Path = path;
		ContentType = contentType;
		Title = title;
	}

	public static Content Create(AggregateRootId id, string path, string contentType, string? title = null) => 
		new(id, path, contentType, title);
}