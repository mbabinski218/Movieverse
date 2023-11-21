using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Domain.AggregateRoots.Media;
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

	private static readonly Func<Context, MediaId, Task<Media?>> getMediaAsync =
		EF.CompileAsyncQuery((Context context, MediaId id) =>
			context.Media.SingleOrDefault(x => x.Id == id));
	
	public async Task<Result<Media>> FindAsync(MediaId id, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Finding media with id {id}...", id.ToString());
		
		var media = await getMediaAsync(_dbContext, id);
		
		return media is null ? Error.NotFound(MediaResources.NotFound) : media;
	}

	private static readonly Func<Context, MediaId, Task<Media?>> getFullMediaAsync =
		EF.CompileAsyncQuery((Context context, MediaId id) =>
			context.Media.Where(m => m.Id == id)
				.Include(m => m.PlatformIds)
				.Include(m => m.Staff)
				.Include(m => m.Genres)
				.ThenInclude(g => g.Media)
				.SingleOrDefault());
	
	public async Task<Result<Media>> FindFullAsync(MediaId id, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Finding media with id {id}...", id.ToString());
		
		var media = await getFullMediaAsync(_dbContext, id);
		
		return media is null ? Error.NotFound(MediaResources.NotFound) : media;
	} 

	public async Task<Result<List<Media>>> GetAllAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting all media");

		return await _dbContext.Media
			.Include(x => x.AdvancedStatistics)
			.ThenInclude(x => x.Popularity)
			.ToListAsync(cancellationToken);
	}

	public async Task<bool> TitleExistsAsync(string title, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Checking if media with title {title} exists...", title);
		
		return await _dbContext.Media.AnyAsync(m => m.Title == title, cancellationToken: cancellationToken);
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