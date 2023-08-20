using Microsoft.AspNetCore.Http;
using Movieverse.Domain.Common;
using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.Domain.DomainEvents;

public sealed record ImageChanged(
	AggregateRootId ImageId, 
	IFormFile NewImage
	) : IDomainEvent;