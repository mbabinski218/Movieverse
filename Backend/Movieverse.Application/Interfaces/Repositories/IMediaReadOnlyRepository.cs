using Movieverse.Contracts.DataTransferObjects.Genre;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Domain.Common;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.Entities;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Application.Interfaces.Repositories;

public interface IMediaReadOnlyRepository
{
	Task<Result<MediaDto>> FindAsync(MediaId id, CancellationToken cancellationToken = default);
	Task<Result<IPaginatedList<MediaDemoDto>>> GetLatestMediaAsync(PlatformId platformId, short? pageNumber = null, short? pageSize = null, CancellationToken cancellationToken = default);
	Task<Result<IPaginatedList<SearchMediaDto>>> FindMediaByIdsAsync(List<MediaId> ids, short? pageNumber, short? pageSize, CancellationToken cancellationToken = default);
	Task<Result<IPaginatedList<MediaInfoDto>>> FindMoviesByIdsAsync(List<MediaId> ids, short? pageNumber, short? pageSize, CancellationToken cancellationToken = default);
	Task<Result<IPaginatedList<MediaInfoDto>>> FindSeriesByIdsAsync(List<MediaId> ids, short? pageNumber, short? pageSize, CancellationToken cancellationToken = default);
	Task<Result<IPaginatedList<SearchMediaDto>>> SearchMediaAsync(string search, short? pageNumber, short? pageSize, CancellationToken cancellationToken = default);
	Task<Result<IPaginatedList<SearchMediaDto>>> SearchMoviesWithFiltersAsync(string? search, int? genre, short? pageNumber, short? pageSize, CancellationToken cancellationToken = default);
	Task<Result<IPaginatedList<SearchMediaDto>>> SearchSeriesWithFiltersAsync(string? search, int? genre, short? pageNumber, short? pageSize, CancellationToken cancellationToken = default);

	Task<Result<IPaginatedList<SearchMediaDto>>> FindMostPopularMoviesAsync(short? pageNumber, short? pageSize, CancellationToken cancellationToken = default);
	Task<Result<IPaginatedList<SearchMediaDto>>> FindMostPopularSeriesAsync(short? pageNumber, short? pageSize, CancellationToken cancellationToken = default);
	Task<Result<IPaginatedList<SearchMediaDto>>> FindMostPopularAsync(string? type, short? pageNumber, short? pageSize, CancellationToken cancellationToken = default);
	
	Task<Result<IPaginatedList<SearchMediaDto>>> FindUpcomingMoviesAsync(short? pageNumber, short? pageSize, CancellationToken cancellationToken = default);
	Task<Result<IPaginatedList<SearchMediaDto>>> FindUpcomingSeriesAsync(short? pageNumber, short? pageSize, CancellationToken cancellationToken = default);
	
	Task<Result<IPaginatedList<SearchMediaDto>>> FindTop100MoviesAsync(short? pageNumber, short? pageSize, CancellationToken cancellationToken = default);
	Task<Result<IPaginatedList<SearchMediaDto>>> FindTop100SeriesAsync(short? pageNumber, short? pageSize, CancellationToken cancellationToken = default);
	
	Task<Result<IEnumerable<GenreInfoDto>>> GetGenresAsync(MediaId id, CancellationToken cancellationToken = default);
	Task<Result<IEnumerable<GenreDto>>> GetAllGenresAsync(CancellationToken cancellationToken = default);
	
	Task<Result<IEnumerable<ContentId>>> GetContentAsync(MediaId id, CancellationToken cancellationToken = default);
	Task<Result<IEnumerable<PlatformId>>> GetPlatformIdsAsync(MediaId id, CancellationToken cancellationToken = default);
	Task<Result<IEnumerable<Staff>>> GetStaffAsync(MediaId id, CancellationToken cancellationToken = default);
	Task<Result<StatisticsDto>> GetStatisticsAsync(MediaId id, CancellationToken cancellationToken = default);
	Task<Result<SeasonInfoDto>> GetSeasonsAsync(MediaId id, CancellationToken cancellationToken = default);
	Task<Result<IEnumerable<MediaSectionDto>>> GetMediaSectionAsync(IEnumerable<MediaId> ids, CancellationToken cancellationToken = default);
	Task<Result<IEnumerable<ReviewDto>>> GetReviewsAsync(MediaId id, CancellationToken cancellationToken = default);
}