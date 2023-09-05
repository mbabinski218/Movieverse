using Movieverse.Domain.Common;
using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.Domain.DomainEvents;

public record GenreToMediaAdded(
	AggregateRootId MediaId,
	AggregateRootId GenreId
	) : IDomainEvent;