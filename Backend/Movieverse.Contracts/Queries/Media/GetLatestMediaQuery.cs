using MediatR;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Queries.Media;

public sealed record GetLatestMediaQuery(
	string? PlaceName,
	Guid? PlaceId, 
	short? PageNumber,
	short? PageSize
	) : IRequest<Result<IEnumerable<FilteredMediaDto>>>;