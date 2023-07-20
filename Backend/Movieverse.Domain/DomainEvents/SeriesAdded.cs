using Movieverse.Domain.Common;
using Movieverse.Domain.ValueObjects;

namespace Movieverse.Domain.DomainEvents;

public record SeriesAdded(ObjectId Id) : IDomainEvent;