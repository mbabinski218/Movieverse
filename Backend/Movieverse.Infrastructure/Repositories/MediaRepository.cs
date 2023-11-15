using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Common;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Common;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.Entities;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;
using Movieverse.Infrastructure.Persistence;

namespace Movieverse.Infrastructure.Repositories;

public sealed class MediaRepository : IMediaRepository
{
	private readonly ILogger<MediaRepository> _logger;
	private readonly Context _dbContext;
	
	public MediaRepository(ILogger<MediaRepository> logger, Context dbContext)
	{
		_logger = logger;
		_dbContext = dbContext;
	}

	public async Task<Result<Media>> FindAsync(MediaId id, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Finding media with id {id}...", id.ToString());
		
		var media = await _dbContext.Media
			.SingleOrDefaultAsync(m => m.Id == id, cancellationToken);
		
		return media is null ? Error.NotFound("Not found") : media;
	}

	public async Task<Result<List<Media>>> GetAllAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting all media");

		return await _dbContext.Media
			.Include(x => x.AdvancedStatistics)
			.ThenInclude(x => x.Popularity)
			.ToListAsync(cancellationToken);
	}
	
	public async Task<Result<IEnumerable<MediaDemoDto>>> GetUpcomingMediaAsync(PlatformId? platformId, short count, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting upcoming media...");
		
		var media = await _dbContext.Media
			.AsNoTracking()
			.Where(m => m.Details.ReleaseDate > DateTime.UtcNow)
			.Where(m => platformId == null || m.PlatformIds.Any(p => p.Value == platformId.Value))
			.OrderBy(m => m.Details.ReleaseDate)
			.Take(count)
			.ProjectToType<MediaDemoDto>()
			.ToListAsync(cancellationToken);
		
		return media;
	}

	public async Task<Result<IEnumerable<MediaDemoDto>>> GetUpcomingMoviesAsync(PlatformId? platformId, short count, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting upcoming movies...");

		var movies = await _dbContext.Movies
			.Where(m => m.Details.ReleaseDate > DateTime.UtcNow)
			.Where(m => platformId == null || m.PlatformIds.Any(p => p == platformId))
			.AsNoTracking()
			.OrderBy(m => m.Details.ReleaseDate)
			.Take(count)
			.ProjectToType<MediaDemoDto>()
			.ToListAsync(cancellationToken);

		return movies;
	}

	public async Task<bool> ExistsAsync(MediaId id, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Checking if media with id {id} exists...", id.ToString());
		
		var media = await FindAsync(id, cancellationToken);
		return media.IsSuccessful;
	}

	public async Task<bool> TitleExistsAsync(string title, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Checking if media with title {title} exists...", title);
		
		return await _dbContext.Media.AnyAsync(m => m.Title == title, cancellationToken: cancellationToken);
	}

	public async Task<Result<IPaginatedList<MediaInfoDto>>> FindMoviesByIdsAsync(List<MediaId> ids, short? pageNumber, short? pageSize, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting movies with ids {ids}...", string.Join(", ", ids.Select(id => id.ToString())));
		
		var idsList = ids.Select(id => id.Value).ToList();
		
		return await _dbContext.Movies
			.Where(m => idsList.Contains(m.Id))
			.AsNoTracking()
			.ProjectToType<MediaInfoDto>()
			.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);
	}

	public async Task<Result<IPaginatedList<MediaInfoDto>>> FindSeriesByIdsAsync(List<MediaId> ids, short? pageNumber, short? pageSize, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting series with ids {ids}...", string.Join(", ", ids.Select(id => id.ToString())));
		
		var idsList = ids.Select(id => id.Value).ToList();
		
		return await _dbContext.Series
			.Where(s => idsList.Contains(s.Id))
			.AsNoTracking()
			.ProjectToType<MediaInfoDto>()
			.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);
	}

	public async Task<Result> AddMovieAsync(Movie media, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Adding media with id {id}...", media.Id.ToString());
		
		await _dbContext.Movies.AddAsync(media, cancellationToken);
		return Result.Ok();
	}
	
	public async Task<Result> AddSeriesAsync(Series media, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Adding media with id {id}...", media.Id.ToString());
		
		await _dbContext.Series.AddAsync(media, cancellationToken);
		return Result.Ok();
	}

	public async Task<Result> UpdateAsync(Media media, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Updating media with id {id}...", media.Id.ToString());

		_dbContext.Media.Update(media);
		return await Task.FromResult(Result.Ok());
	}

	public async Task<Result> UpdateRangeAsync(List<Media> media, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Updating {count} media...", media.Count.ToString());
		
		_dbContext.Media.UpdateRange(media);
		return await Task.FromResult(Result.Ok());
	}

	public async Task<Result<Genre>> FindGenreAsync(int genreId, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Finding genre with id {id}...", genreId.ToString());
		
		var genre = await _dbContext.Genres.FindAsync(new object?[] { genreId }, cancellationToken: cancellationToken);
		
		return genre is not null ? genre : Error.NotFound();
	}

	public async Task<Result> BanReviewsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Banning reviews by user with id {id}...", userId.ToString());

		var media = await _dbContext.Media.Include(media => media.Reviews).ToListAsync(cancellationToken);
		media.ForEach(m =>
		{
			var reviews = m.Reviews.Where(r => r.UserId == userId).ToList();
			reviews.ForEach(r => r.Banned = true);
		});
		
		return Result.Ok();
	}
}