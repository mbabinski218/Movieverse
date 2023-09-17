using Movieverse.Domain.Common;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Domain.DomainEvents;

public record GenreToMediaAdded(
	MediaId MediaId,
	GenreId GenreId
	) : IDomainEvent;