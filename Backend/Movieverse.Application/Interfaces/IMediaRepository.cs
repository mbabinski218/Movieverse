using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Common;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.Application.Interfaces;

public interface IMediaRepository
{
	Task<Result<Media>> FindAsync(AggregateRootId id, CancellationToken cancellationToken = default);
	Task<Result> UpdateStatisticsAsync(CancellationToken cancellationToken = default);
	Task<bool> ExistsAsync(AggregateRootId id, CancellationToken cancellationToken = default);
	Task<bool> TitleExistsAsync(string title, CancellationToken cancellationToken = default);
	Task<Result<IPaginatedList<MediaInfoDto>>> FindMoviesByIdsAsync(List<AggregateRootId> ids, short? pageNumber, short? pageSize, CancellationToken cancellationToken = default);
	Task<Result<IPaginatedList<MediaInfoDto>>> FindSeriesByIdsAsync(List<AggregateRootId> ids, short? pageNumber, short? pageSize, CancellationToken cancellationToken = default);
	Task<Result> AddMovieAsync(Movie media, CancellationToken cancellationToken = default);
	Task<Result> AddSeriesAsync(Series media, CancellationToken cancellationToken = default);
}