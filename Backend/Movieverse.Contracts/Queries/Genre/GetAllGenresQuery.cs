using MediatR;
using Movieverse.Contracts.DataTransferObjects.Genre;
using Movieverse.Domain.Common;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Queries.Genre;

public sealed record GetAllGenresQuery(
	short? PageNumber, 
	short? PageSize
	) : IRequest<Result<IPaginatedList<GenreDto>>>;