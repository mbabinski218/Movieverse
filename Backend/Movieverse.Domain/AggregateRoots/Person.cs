using Movieverse.Domain.Common.Models;
using Movieverse.Domain.ValueObjects;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Domain.AggregateRoots;

public sealed class Person : AggregateRoot<PersonId, Guid>
{
	// Map to table
	private readonly List<ContentId> _contentIds = new();
	private readonly List<MediaId> _mediaIds = new();

	public Information Information { get; set; } = null!;
	public LifeHistory LifeHistory { get; set; } = null!;
	public string? Biography { get; set; }
	public string? FunFacts { get; set; }
	public ContentId? PictureId { get; set; }
	public IReadOnlyList<ContentId> ContentIds => _contentIds.AsReadOnly();
	public IReadOnlyList<MediaId> MediaIds => _mediaIds.AsReadOnly();
		
	// Constructors
	private Person(PersonId id) : base(id)
	{
		Information = new();
		LifeHistory = new();
	}
	
	private Person(PersonId id, Information information, LifeHistory lifeHistory, string? biography, string? funFacts) : base(id)
	{
		Information = information;
		LifeHistory = lifeHistory;
		Biography = biography;
		FunFacts = funFacts;
	}
	
	// Methods
	public static Person Create() => new(PersonId.Create());
	
	public static Person Create(PersonId id) => new(id);
	
	public static Person Create(Information information, LifeHistory lifeHistory, string? biography, string? funFacts) =>
		new(PersonId.Create(), information, lifeHistory, biography, funFacts);
	
	public void AddContent(ContentId contentId)
	{
		_contentIds.Add(contentId);
	}
	
	public void RemoveContent(ContentId contentId)
	{
		_contentIds.Remove(contentId);
	}
	
	public void AddMedia(MediaId mediaId)
	{
		_mediaIds.Add(mediaId);
	}
	
	public void RemoveMedia(MediaId mediaId)
	{
		_mediaIds.Remove(mediaId);
	}

	// Equality
	public override bool Equals(object? obj) => obj is Person entity && Id.Equals(entity.Id);

	public override int GetHashCode() => Id.GetHashCode();

	// EF Core
	private Person()
	{
		
	}
}