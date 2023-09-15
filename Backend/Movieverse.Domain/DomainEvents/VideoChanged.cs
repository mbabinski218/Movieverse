using Movieverse.Domain.Common;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Domain.DomainEvents;

public sealed record VideoChanged(
	ContentId VideoId, 
	string NewVideo
) : IDomainEvent;