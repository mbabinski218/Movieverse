using Movieverse.Domain.Common;

namespace Movieverse.Domain.DomainEvents;

public sealed record RatingChanged(Guid UserId, short NewRating) : IDomainEvent;