using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Domain.Common;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Application.Interfaces.Repositories;

public interface IMediaReadOnlyRepository
{
	Task<Result<MediaDto>> FindAsync(MediaId id, CancellationToken cancellationToken = default);
	Task<Result<IEnumerable<MediaDemoDto>>> GetUpcomingMediaAsync(PlatformId? platformId, short count, CancellationToken cancellationToken = default);
	Task<Result<IEnumerable<MediaDemoDto>>> GetUpcomingMoviesAsync(PlatformId? platformId, short count, CancellationToken cancellationToken = default);
	Task<Result<IPaginatedList<MediaInfoDto>>> FindMoviesByIdsAsync(List<MediaId> ids, short? pageNumber, short? pageSize, CancellationToken cancellationToken = default);
	Task<Result<IPaginatedList<MediaInfoDto>>> FindSeriesByIdsAsync(List<MediaId> ids, short? pageNumber, short? pageSize, CancellationToken cancellationToken = default);
}