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
	public string? PosterPath { get; set; }
	public string? TrailerPath { get; set; }
	public virtual List<ObjectId> PlatformIds { get; set; } = new();
	public virtual List<ObjectId> ContentIds { get; set; } = new();
	public virtual List<MediaGenre> Genres { get; set; } = new();
	public virtual List<Staff> Staff { get; set; } = new();
}