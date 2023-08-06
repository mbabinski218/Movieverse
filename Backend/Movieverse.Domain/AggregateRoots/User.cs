using Movieverse.Domain.Common.Models;
using Movieverse.Domain.DomainEvents;
using Movieverse.Domain.Entities;
using Movieverse.Domain.ValueObjects;
using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.Domain.AggregateRoots;

public class User : IdentityAggregateRoot
{
	// Map to table
	public Information Information { get; set; } = null!;
	public AggregateRootId? AvatarId { get; set; } = null!;
	public virtual List<MediaInfo> MediaInfos { get; private set; } = new();
	public AggregateRootId? PersonId { get; set; }

	// EF Core
	private User()
	{
		
	}
	
	// Other
	
	public static User Create(string email, string userName, string? firstName, string? lastName, ushort age)
	{
		var user = new User
		{
			Id = Guid.NewGuid(),
			Email = email,
			UserName = userName,
			Information = new Information
			{
				FirstName = firstName,
				LastName = lastName,
				Age = age
			}
		};

		user.AddDomainEvent(new UserRegistered(AggregateRootId.Create(user.Id), user.Email));

		return user;
	}
}