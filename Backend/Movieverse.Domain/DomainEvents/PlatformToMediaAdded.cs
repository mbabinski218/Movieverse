using Movieverse.Domain.Common;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Domain.DomainEvents;

public sealed record PlatformToMediaAdded(
	MediaId MediaId,
	PlatformId PlatformId
	) : IDomainEvent;