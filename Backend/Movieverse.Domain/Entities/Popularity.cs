using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Models;
using Movieverse.Domain.ValueObjects;

namespace Movieverse.Domain.Entities;

public class Popularity : Entity<ObjectId>
{
	public virtual Statistics Statistics { get; set; } = null!;
	public ObjectId StatisticsId { get; set; } = null!;
	public DateTimeOffset Date { get; set; }
	public int Position { get; set; }
	public int Change { get; set; }
}