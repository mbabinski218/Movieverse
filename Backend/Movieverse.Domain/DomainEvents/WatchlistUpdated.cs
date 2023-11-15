using Movieverse.Domain.Common;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Domain.DomainEvents;

public sealed record WatchlistUpdated(MediaId MediaId, bool Status) : IDomainEvent;