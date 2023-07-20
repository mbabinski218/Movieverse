using Movieverse.Domain.Common.Models;
using Movieverse.Domain.ValueObjects;

namespace Movieverse.Domain.AggregateRoots;

public class Person : AggregateRoot<ObjectId>
{
	public Information Information { get; set; } = null!;
	public LifeHistory LifeHistory { get; set; } = null!;
	public string? Biography { get; set; }
	public string? FunFacts { get; set; }
	public List<ObjectId> ContentIds { get; set; } = new();
	public List<Staff> Staff { get; set; } = new();
}