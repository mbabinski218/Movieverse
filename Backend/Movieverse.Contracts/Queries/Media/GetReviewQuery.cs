using MediatR;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Queries.Media;

public sealed record GetReviewQuery(
	Guid Id
	) : IRequest<Result<IEnumerable<ReviewDto>>>;