using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.ValueObjects;

namespace Movieverse.Domain.Entities;

public class StatisticsAward
{
	public virtual Statistics Statistics { get; set; } = null!;
	public ObjectId StatisticsId { get; set; } = null!;
	public virtual Award Award { get; set; } = null!;
	public ObjectId AwardId { get; set; } = null!;
	public int Year { get; set; }
	public int Place { get; set; }
}