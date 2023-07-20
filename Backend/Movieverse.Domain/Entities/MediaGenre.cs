using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.ValueObjects;

namespace Movieverse.Domain.Entities;

public class MediaGenre
{
	public ObjectId MediaId { get; set; } = null!;
	public virtual Media Media { get; set; } = null!;
	public ObjectId GenreId { get; set; } = null!;
	public virtual Genre Genre { get; set; } = null!;
}