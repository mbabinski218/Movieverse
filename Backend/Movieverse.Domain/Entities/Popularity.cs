using Movieverse.Domain.Common.Models;
using Movieverse.Domain.ValueObjects;

namespace Movieverse.Domain.Entities;

public class Popularity : Entity
{
	// Map to table
	public virtual Statistics Statistics { get; private set; } = null!;
	public BasicStatistics BasicStatistics { get; set; } = null!;
	public DateTimeOffset Date { get; set; }
	public uint? Position { get; set; }
	public uint? Change { get; set; }

	// EF Core
	private Popularity()
	{
			
	}
	
	// Other
}