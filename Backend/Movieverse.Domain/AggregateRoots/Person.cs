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

	private Person(AggregateRootId id, Information information, LifeHistory lifeHistory, string? biography, string? funFacts) : base(id)
	{
		Information = information;
		LifeHistory = lifeHistory;
		Biography = biography;
		FunFacts = funFacts;
	}
	
	// Other
	public static Person Create(AggregateRootId id, Information information, LifeHistory lifeHistory, string? biography, string? funFacts) =>
		new(id, information, lifeHistory, biography, funFacts);
	
	public static Person Create(Information information, LifeHistory lifeHistory, string? biography, string? funFacts) =>
		new(AggregateRootId.Create(), information, lifeHistory, biography, funFacts);
}