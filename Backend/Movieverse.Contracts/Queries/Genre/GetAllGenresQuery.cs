using MediatR;
using Movieverse.Contracts.DataTransferObjects.Genre;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Queries.Genre;

public sealed record GetAllGenresQuery: IRequest<Result<IEnumerable<GenreDto>>>;