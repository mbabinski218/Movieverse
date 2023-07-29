using Movieverse.Domain.Common;

namespace Movieverse.Domain.DomainEvents;

public sealed record SeriesAdded(Guid Id) : IDomainEvent;