using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Common.Models;
using Movieverse.Domain.ValueObjects;

namespace Movieverse.Domain.Entities;

public class Statistics : Entity
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
	
	// Other
}