using Movieverse.Domain.Common.Models;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Domain.AggregateRoots;

public class Platform : AggregateRoot<PlatformId, Guid>
{
	// Map to table
	public string Name { get; set; } = null!;
	public ContentId LogoId { get; set; } = null!;
	public decimal Price { get; set; }
	public virtual List<MediaId> MediaIds { get; private set; } = new();

	// EF Core
	private Platform()
	{

	}
	
	// Other
	private Platform(PlatformId id, string name, ContentId logoId, decimal price) : base(id)
	{
		Name = name;
		LogoId = logoId;
		Price = price;
	}

	public static Platform Create(string name, ContentId logoId, decimal price)
		=> new(PlatformId.Create(), name, logoId, price);
	
	public static Platform Create(PlatformId id, string name, ContentId logoId, decimal price)
		=> new(id, name, logoId, price);
}