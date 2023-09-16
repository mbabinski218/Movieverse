using Movieverse.Domain.Common.Models;
using Movieverse.Domain.Entities;
using Movieverse.Domain.ValueObjects;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Domain.AggregateRoots;

public class User : IdentityAggregateRoot
{
	// Map to table
	private readonly List<MediaInfo> _mediaInfos = new();
	
	public Information Information { get; set; } = null!;
	public ContentId? AvatarId { get; set; } = null!;
	public IReadOnlyList<MediaInfo> MediaInfos => _mediaInfos.AsReadOnly();
	public PersonId? PersonId { get; set; }

	// Constructors
	protected User(Guid id, string email, string userName, string? firstName, string? lastName, short age)
	{
		Id = id;
		Email = email;
		UserName = userName;
		Information = new Information
		{
			FirstName = firstName,
			LastName = lastName,
			Age = age
		};
	}
	
	// Methods
	public static User Create(string email, string userName, string? firstName, string? lastName, short age) => 
		new(Guid.NewGuid(), email, userName, firstName, lastName, age);
	
	public void AddMediaInfo(MediaInfo mediaInfo)
	{
		_mediaInfos.Add(mediaInfo);
	}
	
	public void RemoveMediaInfo(MediaInfo mediaInfo)
	{
		_mediaInfos.Remove(mediaInfo);
	}
	
	// EF Core
	private User()
	{
		
	}
}