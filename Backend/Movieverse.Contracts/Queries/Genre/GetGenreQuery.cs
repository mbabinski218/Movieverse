using MediatR;
using Movieverse.Contracts.DataTransferObjects.Genre;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Queries.Genre;

public sealed record GetGenreQuery(Guid Id) : IRequest<Result<GenreDto>>;