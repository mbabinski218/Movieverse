using Movieverse.Domain.Common;
using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.Domain.DomainEvents;

public sealed record PersonalityCreated(
	AggregateRootId PersonId, 
	AggregateRootId UserId
	) : IDomainEvent;