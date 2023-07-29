using Movieverse.Domain.Common.Models;

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
}