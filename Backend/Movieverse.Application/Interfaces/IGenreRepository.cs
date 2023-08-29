using Movieverse.Contracts.DataTransferObjects.Genre;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.Application.Interfaces;

public interface IGenreRepository
{
	Task<Result> AddAsync(Genre genre, CancellationToken cancellationToken = default);
	Task<Result<Genre>> FindByIdAsync(AggregateRootId id, CancellationToken cancellationToken = default);
	Task<Result<IPaginatedList<GenreDto>>> GetAllAsync(short? pageNumber = null, short? pageSize = null, CancellationToken cancellationToken = default);
}