using Movieverse.Contracts.DataTransferObjects.Genre;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Application.Interfaces.Repositories;

public interface IGenreReadOnlyRepository
{
	Task<Result<GenreDto>> FindAsync(GenreId id, CancellationToken cancellationToken = default);
	Task<Result<IEnumerable<GenreDto>>> GetAllAsync(CancellationToken cancellationToken = default);
	Task<Result<IEnumerable<GenreInfoDto>>> GetGenresInfoAsync(IEnumerable<GenreId> ids, CancellationToken cancellationToken = default);
}