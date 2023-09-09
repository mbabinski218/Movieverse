using Movieverse.Domain.Common;
using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.Domain.DomainEvents;

public sealed record PersonToMediaAdded(
	AggregateRootId MediaId,
	AggregateRootId PersonId
	) : IDomainEvent;