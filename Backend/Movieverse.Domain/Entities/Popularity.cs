using Movieverse.Domain.Common.Models;
using Movieverse.Domain.ValueObjects;

namespace Movieverse.Domain.Entities;

public class Popularity : Entity<int>
{
	// Map to table
	public virtual Statistics Statistics { get; private set; } = null!;
	public BasicStatistics BasicStatistics { get; set; } = null!;
	public DateTimeOffset Date { get; set; }
	public int Position { get; set; }
	public int Change { get; set; }
	public long Views { get; set; }
	
	// Constructors
	private Popularity(DateTimeOffset date)
	{
		BasicStatistics = new BasicStatistics();
		Date = date;
		Position = 0;
		Change = 0;
		Views = 0;
	}

	// Methods
	public static Popularity Create(Statistics statistics, DateTimeOffset date)
	{
		var popularity = new Popularity(date)
		{
			Statistics = statistics,
		};
		return popularity;
	}
	
	// EF Core
	private Popularity()
	{
			
	}
}