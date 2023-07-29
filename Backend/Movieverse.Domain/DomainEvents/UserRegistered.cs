using Movieverse.Domain.Common;
using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.Domain.DomainEvents;

public sealed record UserRegistered(AggregateRootId UserId, string Email) : IDomainEvent;