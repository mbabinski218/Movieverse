using Movieverse.Domain.Common.Models;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Domain.AggregateRoots;

public sealed class Platform : AggregateRoot<PlatformId, Guid>
{
	// Map to table
	private readonly List<MediaId> _mediaIds = new();
	
	public string Name { get; set; } = null!;
	public decimal Price { get; set; }
	public IReadOnlyList<MediaId> MediaIds => _mediaIds.AsReadOnly();

	// Constructors
	private Platform(PlatformId id, string name, decimal price) : base(id)
	{
		Name = name;
		Price = price;
	}

	//Methods
	public static Platform Create(PlatformId id, string name, decimal price)
		=> new(id, name, price);
	
	public static Platform Create(string name, decimal price)
		=> Create(PlatformId.Create(), name, price);
	
	public static Platform Create(string name, ContentId logoId, decimal price)
		=> new(PlatformId.Create(), name, price);
	
	public static Platform Create(PlatformId id, string name, ContentId logoId, decimal price)
		=> new(id, name, price);
	
	public void AddMedia(MediaId mediaId)
	{
		_mediaIds.Add(mediaId);
	}
	
	public void RemoveMedia(MediaId mediaId)
	{
		_mediaIds.Remove(mediaId);
	}

	// Equality
	public override bool Equals(object? obj) => obj is Platform entity && Id.Equals(entity.Id);

	public override int GetHashCode() => Id.GetHashCode();
	
	// EF Core
	private Platform()
	{

	}
}