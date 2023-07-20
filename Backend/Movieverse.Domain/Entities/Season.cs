using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Models;
using Movieverse.Domain.ValueObjects;

namespace Movieverse.Domain.Entities;

public class Season : Entity<ObjectId>
{
	public virtual Series Series { get; set; } = null!;
	public ObjectId SeriesId { get; set; } = null!;
	public int SeasonNumber { get; set; }
	public virtual List<Episode> Episodes { get; set; } = new();
	public int? EpisodeCount { get; set; }
}