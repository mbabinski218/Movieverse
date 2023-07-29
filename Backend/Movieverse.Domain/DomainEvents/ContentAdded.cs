using Movieverse.Domain.Common;

namespace Movieverse.Domain.DomainEvents;

public sealed record ContentAdded(Guid Id) : IDomainEvent;