using Movieverse.Domain.Common;

namespace Movieverse.Domain.DomainEvents;

public sealed record EpisodeAdded(Guid Id) : IDomainEvent;