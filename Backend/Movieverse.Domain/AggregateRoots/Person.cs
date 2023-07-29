using Movieverse.Domain.Common.Models;
using Movieverse.Domain.ValueObjects;
using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.Domain.AggregateRoots;

public class Person : AggregateRoot
{
	// Map to table
	public Information Information { get; set; } = null!;
	public LifeHistory LifeHistory { get; set; } = null!;
	public string? Biography { get; set; }
	public string? FunFacts { get; set; }
	public virtual List<AggregateRootId> ContentIds { get; private set; } = new();

	// EF Core
	private Person()
	{
		
	}
	
	// Other
}