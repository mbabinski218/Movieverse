namespace Movieverse.Domain.Entities;

public class StatisticsAward
{
	// Map to table
	public virtual Statistics Statistics { get; set; } = null!;
	public virtual Award Award { get; set; } = null!;
	public ushort Year { get; set; }
	public ushort Place { get; set; }

	// EF Core
	private StatisticsAward()
	{
		
	}
}