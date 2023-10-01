using Movieverse.Domain.Common;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Domain.DomainEvents;

public sealed record MediaToPlatformAdded(
	PlatformId PlatformId,
	MediaId MediaId
	) : IDomainEvent;
