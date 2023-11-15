using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Domain.Entities;

public class MediaGenre
{
	public virtual Media Media { get; set; } = null!;
	public MediaId MediaId { get; set; } = null!;
	public virtual Genre Genre { get; set; } = null!;
	public int GenreId { get; set; }
}