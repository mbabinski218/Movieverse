using Movieverse.Contracts.DataTransferObjects.Genre;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Application.Interfaces.Repositories;

public interface IGenreRepository
{
	Task<Result> AddAsync(Genre genre, CancellationToken cancellationToken = default);
	Task<Result<Genre>> FindAsync(GenreId id, CancellationToken cancellationToken = default);
	Task<Result<IPaginatedList<GenreDto>>> GetAllAsync(short? pageNumber = null, short? pageSize = null, CancellationToken cancellationToken = default);
}