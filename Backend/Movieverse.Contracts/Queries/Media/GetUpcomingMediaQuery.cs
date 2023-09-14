using MediatR;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Queries.Media;

public record GetUpcomingMediaQuery(
	string Place, 
	short Count
	) : IRequest<Result<IEnumerable<FilteredMediaDto>>>;
