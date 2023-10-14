using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Common;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Common;
using Movieverse.Domain.Common.Result;
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
		_logger.LogDebug("Finding media with id {id}...", id.ToString());
		
		var media = await _dbContext.Medias
			.FirstOrDefaultAsync(m => m.Id == id, cancellationToken)
			;
		
		return media is null ? Error.NotFound("Not found") : media;
	}

	public async Task<Result<List<Media>>> GetAllAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Getting all media");

		return await _dbContext.Medias
			.Include(x => x.AdvancedStatistics)
			.ThenInclude(x => x.Popularity)
			.ToListAsync(cancellationToken)
			;
	}
	
	public async Task<Result<IEnumerable<MediaDemoDto>>> GetUpcomingMediaAsync(PlatformId? platformId, short count, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Getting upcoming medias...");
		
		var medias = await _dbContext.Medias
			.AsNoTracking()
			.Where(m => m.Details.ReleaseDate > DateTime.UtcNow)
			.Where(m => platformId == null || m.PlatformIds.Any(p => p.Value == platformId.Value))
			.OrderBy(m => m.Details.ReleaseDate)
			.Take(count)
			.ProjectToType<MediaDemoDto>()
			.ToListAsync(cancellationToken)
			;
		
		return medias;
	}

	public async Task<Result<IEnumerable<MediaDemoDto>>> GetUpcomingMoviesAsync(PlatformId? platformId, short count, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Getting upcoming movies...");

		var movies = await _dbContext.Movies
			.Where(m => m.Details.ReleaseDate > DateTime.UtcNow)
			.Where(m => platformId == null || m.PlatformIds.Any(p => p == platformId))
			.AsNoTracking()
			.OrderBy(m => m.Details.ReleaseDate)
			.Take(count)
			.ProjectToType<MediaDemoDto>()
			.ToListAsync(cancellationToken)
			;

		return movies;
	}

	public async Task<bool> ExistsAsync(MediaId id, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Checking if media with id {id} exists...", id.ToString());
		
		var media = await FindAsync(id, cancellationToken);
		return media.IsSuccessful;
	}

	public async Task<bool> TitleExistsAsync(string title, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Checking if media with title {title} exists...", title);
		
		return await _dbContext.Medias.AnyAsync(m => m.Title == title, cancellationToken: cancellationToken);
	}

	public async Task<Result<IPaginatedList<MediaInfoDto>>> FindMoviesByIdsAsync(List<MediaId> ids, short? pageNumber, short? pageSize, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Getting movies with ids {ids}...", string.Join(", ", ids.Select(id => id.ToString())));
		
		var idsList = ids.Select(id => id.Value).ToList();
		
		return await _dbContext.Movies
			.Where(m => idsList.Contains(m.Id))
			.AsNoTracking()
			.ProjectToType<MediaInfoDto>()
			.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken)
			;
	}

	public async Task<Result<IPaginatedList<MediaInfoDto>>> FindSeriesByIdsAsync(List<MediaId> ids, short? pageNumber, short? pageSize, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Getting series with ids {ids}...", string.Join(", ", ids.Select(id => id.ToString())));
		
		var idsList = ids.Select(id => id.Value).ToList();
		
		return await _dbContext.Series
			.Where(s => idsList.Contains(s.Id))
			.AsNoTracking()
			.ProjectToType<MediaInfoDto>()
			.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken)
			;
	}

	public async Task<Result> AddMovieAsync(Movie media, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Adding media with id {id}...", media.Id.ToString());
		
		await _dbContext.Movies.AddAsync(media, cancellationToken);
		return Result.Ok();
	}
	
	public async Task<Result> AddSeriesAsync(Series media, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Adding media with id {id}...", media.Id.ToString());
		
		await _dbContext.Series.AddAsync(media, cancellationToken);
		return Result.Ok();
	}

	public async Task<Result> UpdateAsync(Media media, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Updating media with id {id}...", media.Id.ToString());

		_dbContext.Medias.Update(media);
		return await Task.FromResult(Result.Ok());
	}

	public async Task<Result> UpdateRangeAsync(List<Media> medias, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Updating {count} medias...", medias.Count.ToString());
		
		_dbContext.Medias.UpdateRange(medias);
		return await Task.FromResult(Result.Ok());
	}
}