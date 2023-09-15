using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Common.Models;
using Movieverse.Domain.ValueObjects;

namespace Movieverse.Domain.Entities;

public class Statistics : Entity<int>
{
	// Map to table
	public virtual Media Media { get; private set; } = null!;
	public BoxOffice BoxOffice { get; set; } = null!;
	public virtual List<Popularity> Popularity { get; private set; } = new();
	public virtual List<StatisticsAward> StatisticsAwards { get; private set; } = new();

	// EF Core
	private Statistics()
	{
		
	}
	
	private Statistics(Media media)
	{
		Media = media;
		BoxOffice = new BoxOffice();
	}
	
	// Other
	public static Statistics Create(Media media)
	{
		var statistics = new Statistics(media);
		statistics.Popularity.Add(Entities.Popularity.Create(statistics, DateTimeOffset.UtcNow));
		return statistics;
	}
}