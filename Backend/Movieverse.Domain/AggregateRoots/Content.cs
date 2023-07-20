using Movieverse.Domain.Common.Models;
using Movieverse.Domain.ValueObjects;

namespace Movieverse.Domain.AggregateRoots;

public class Content : AggregateRoot<ObjectId>
{
	public string Path { get; set; } = null!;
	public string ContentType { get; set; } = null!;
	public string? Title { get; set; }
	public DateTimeOffset CreationTime { get; set; }
}