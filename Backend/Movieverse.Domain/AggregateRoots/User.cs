using Movieverse.Domain.Common.Models;
using Movieverse.Domain.Entities;
using Movieverse.Domain.ValueObjects;
using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.Domain.AggregateRoots;

public class User : IdentityAggregateRoot
{
	// Map to table
	public Information Information { get; set; } = null!;
	public AggregateRootId? AvatarId { get; set; } = null!;
	public List<MediaInfo> MediaInfos { get; private set; } = new();
	public AggregateRootId? PersonId { get; set; }

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