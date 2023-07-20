using Movieverse.Domain.Common.Models;
using Movieverse.Domain.Entities;
using Movieverse.Domain.ValueObjects;

namespace Movieverse.Domain.AggregateRoots;

public class Statistics : AggregateRoot<ObjectId>
{
	public ObjectId MediaId { get; set; } = null!;
	public BasicStatistics Current { get; set; } = null!;
	public BasicStatistics Previous { get; set; } = null!;
	public BoxOffice BoxOffice { get; set; } = null!;
	public virtual List<Popularity> Popularity { get; set; } = new();
	public virtual List<StatisticsAward> StatisticsAwards { get; set; } = new();
}