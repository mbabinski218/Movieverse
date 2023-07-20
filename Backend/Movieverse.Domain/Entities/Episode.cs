using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Models;
using Movieverse.Domain.ValueObjects;

namespace Movieverse.Domain.Entities;

public class Episode : Entity<ObjectId>
{
	public virtual Season Season { get; set; } = null!;
	public ObjectId SeasonId { get; set; } = null!;
	public int EpisodeNumber { get; set; }
	public string Title { get; set; } = null!;
	public Details Details { get; set; } = null!;
	public BasicStatistics BasicStatistics { get; set; } = null!;
	public List<ObjectId> ContentIds { get; set; } = new();
}