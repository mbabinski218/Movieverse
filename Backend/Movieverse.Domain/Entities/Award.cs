using Movieverse.Domain.Common.Models;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Domain.Entities;

public sealed class Award : Entity<int>
{
	// Map to table
	private readonly List<StatisticsAward> _statisticsAwards = new();
	
	public string Name { get; set; } = null!;
	public string? Description { get; set; }
	public ContentId? ImageId { get; set; }
	public IReadOnlyList<StatisticsAward> StatisticsAwards => _statisticsAwards.AsReadOnly();
	
	// Methods
	public void AddStatisticsAward(StatisticsAward statisticsAward)
	{
		_statisticsAwards.Add(statisticsAward);
	}
	
	public void RemoveStatisticsAward(StatisticsAward statisticsAward)
	{
		_statisticsAwards.Remove(statisticsAward);
	}
	
	// EF Core
	private Award()
	{
		
	}
}