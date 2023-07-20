using Movieverse.Domain.Common.Models;
using Movieverse.Domain.ValueObjects;

namespace Movieverse.Domain.Entities;

public class Genre : Entity<ObjectId>
{
	public string Name { get; set; } = null!;
	public string Description { get; set; } = null!;
	public int MediaCount { get; set; }
	public virtual List<MediaGenre> Media { get; set; } = new();
}