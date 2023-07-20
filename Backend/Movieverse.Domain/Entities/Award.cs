using Movieverse.Domain.Common.Models;
using Movieverse.Domain.ValueObjects;

namespace Movieverse.Domain.Entities;

public class Award : Entity<ObjectId>
{
	public string Name { get; set; } = null!;
	public string? Description { get; set; }
	public ObjectId? ImageId { get; set; }
	public virtual List<StatisticsAward> StatisticsAwards { get; set; } = new();
}