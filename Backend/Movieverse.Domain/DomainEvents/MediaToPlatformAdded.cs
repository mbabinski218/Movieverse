using Movieverse.Domain.Common;
using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.Domain.DomainEvents;

public sealed record MediaToPlatformAdded(
	AggregateRootId PlatformId,
	AggregateRootId MediaId
	) : IDomainEvent;
