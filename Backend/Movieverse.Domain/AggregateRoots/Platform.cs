using Movieverse.Domain.Common.Models;
using Movieverse.Domain.ValueObjects;

namespace Movieverse.Domain.AggregateRoots;

public class Platform : AggregateRoot<ObjectId>
{
	public string Name { get; set; } = null!;
	public ObjectId LogoId { get; set; } = null!;
	public decimal Price { get; set; }
}