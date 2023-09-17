using Movieverse.Contracts.DataTransferObjects.Genre;
using Movieverse.Domain.Common;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Application.Interfaces.Repositories;

public interface IGenreReadOnlyRepository
{
	Task<Result<GenreDto>> FindAsync(GenreId id, CancellationToken cancellationToken = default);
	Task<Result<IPaginatedList<GenreDto>>> GetAllAsync(short? pageNumber = null, short? pageSize = null, CancellationToken cancellationToken = default);
}