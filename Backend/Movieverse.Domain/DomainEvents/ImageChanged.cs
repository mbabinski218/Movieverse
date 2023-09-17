using Microsoft.AspNetCore.Http;
using Movieverse.Domain.Common;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Domain.DomainEvents;

public sealed record ImageChanged(
	ContentId ImageId, 
	IFormFile NewImage
	) : IDomainEvent;