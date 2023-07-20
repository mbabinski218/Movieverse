using Movieverse.Domain.Common.Models;
using Movieverse.Domain.Entities;
using Movieverse.Domain.ValueObjects;

namespace Movieverse.Domain.AggregateRoots;

public class Media : AggregateRoot<ObjectId>
{
	public string Title { get; set; } = null!;
	public Details Details { get; set; } = null!;
	public TechnicalSpecs TechnicalSpecs { get; set; } = null!;
	public ObjectId StatisticsId { get; set; } = null!;
	public int Popularity { get; set; }
	public virtual List<MediaGenre> Genres { get; set; } = new();
	public string? PosterPath { get; set; }
	public string? TrailerPath { get; set; }
	public List<ObjectId> ContentIds { get; set; } = new();
}