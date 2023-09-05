using Movieverse.Domain.Common;
using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.Domain.DomainEvents;

public record PlatformToMediaAdded(
	AggregateRootId MediaId,
	AggregateRootId PlatformId
	) : IDomainEvent;