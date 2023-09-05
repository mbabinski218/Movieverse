﻿using Movieverse.Domain.Common;
using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.Domain.DomainEvents;

public sealed record VideoChanged(
	AggregateRootId VideoId, 
	string NewVideo
) : IDomainEvent;