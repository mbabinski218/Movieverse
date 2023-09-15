using Movieverse.Domain.Common.Models;
using Movieverse.Domain.Entities;
using Movieverse.Domain.ValueObjects;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Domain.AggregateRoots;

public class User : IdentityAggregateRoot
{
	// Map to table
	public Information Information { get; set; } = null!;
	public ContentId? AvatarId { get; set; } = null!;
	public List<MediaInfo> MediaInfos { get; private set; } = new();
	public PersonId? PersonId { get; set; }

	// EF Core
	private User()
	{
		
	}

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
	
	// Other
	public static User Create(string email, string userName, string? firstName, string? lastName, short age) => 
		new(Guid.NewGuid(), email, userName, firstName, lastName, age);
}