using Movieverse.Domain.Common;

namespace Movieverse.Domain.DomainEvents;

public sealed record UserBanned(Guid UserId) : IDomainEvent;