using Movieverse.Domain.Common.Models;
using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.Domain.AggregateRoots;

public class Platform : AggregateRoot
{
	// Map to table
	public string Name { get; set; } = null!;
	public AggregateRootId LogoId { get; set; } = null!;
	public decimal Price { get; set; }
	public virtual List<AggregateRootId> MediaIds { get; private set; } = new();

	// EF Core
	private Platform()
	{

	}
	
	// Other
	private Platform(AggregateRootId id, string name, AggregateRootId logoId, decimal price) : base(id)
	{
		Name = name;
		LogoId = logoId;
		Price = price;
	}

	public static Platform Create(string name, AggregateRootId logoId, decimal price)
		=> new(AggregateRootId.Create(Guid.NewGuid()), name, logoId, price);
}