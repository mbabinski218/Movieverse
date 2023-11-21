using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.Entities;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Application.Interfaces.Repositories;

public interface IMediaRepository
{
	Task<Result<Media>> FindAsync(MediaId id, CancellationToken cancellationToken = default);
	Task<Result<Media>> FindFullAsync(MediaId id, CancellationToken cancellationToken = default);
	Task<Result<List<Media>>> GetAllAsync(CancellationToken cancellationToken = default);
	Task<bool> TitleExistsAsync(string title, CancellationToken cancellationToken = default);
	Task<Result> AddMovieAsync(Movie media, CancellationToken cancellationToken = default);
	Task<Result> AddSeriesAsync(Series media, CancellationToken cancellationToken = default);
	Task<Result> UpdateAsync(Media media, CancellationToken cancellationToken = default);
	Task<Result> UpdateRangeAsync(List<Media> media, CancellationToken cancellationToken = default);
	Task<Result<Genre>> FindGenreAsync(int genreId, CancellationToken cancellationToken = default);
	Task<Result> BanReviewsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}