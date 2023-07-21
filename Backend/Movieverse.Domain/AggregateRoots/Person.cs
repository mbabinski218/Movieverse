using Movieverse.Domain.Common.Models;
using Movieverse.Domain.Entities;
using Movieverse.Domain.ValueObjects;

namespace Movieverse.Domain.AggregateRoots;

public class Person : AggregateRoot<ObjectId>
{
	public Information Information { get; set; } = null!;
	public LifeHistory LifeHistory { get; set; } = null!;
	public string? Biography { get; set; }
	public string? FunFacts { get; set; }
	public virtual List<ObjectId> ContentIds { get; set; } = new();
}