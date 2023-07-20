using Movieverse.Domain.Common;
using Movieverse.Domain.ValueObjects;

namespace Movieverse.Domain.DomainEvents;

public record RatingChanged(ObjectId UserId, short NewRating) : IDomainEvent;