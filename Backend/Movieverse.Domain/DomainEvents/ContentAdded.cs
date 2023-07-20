using Movieverse.Domain.Common;
using Movieverse.Domain.ValueObjects;

namespace Movieverse.Domain.DomainEvents;

public record ContentAdded(ObjectId Id) : IDomainEvent;