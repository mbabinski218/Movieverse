using MediatR;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Queries.Media;

public sealed record GetUpcomingMediaQuery(
	string Place, 
	short Count
	) : IRequest<Result<IEnumerable<FilteredMediaDto>>>;
