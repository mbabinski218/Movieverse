using Movieverse.Domain.Common;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Domain.DomainEvents;

public sealed record PersonToMediaAdded(
	MediaId MediaId,
	PersonId PersonId
	) : IDomainEvent;