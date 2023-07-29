using Movieverse.Domain.Common;

namespace Movieverse.Domain.DomainEvents;

public sealed record MovieAdded(Guid Id) : IDomainEvent;