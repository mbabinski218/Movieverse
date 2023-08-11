using Movieverse.Domain.Common;

namespace Movieverse.Domain.DomainEvents;

public sealed record UserRegistered(
	Guid UserId, 
	string Email, 
	string Token)
	: IDomainEvent;