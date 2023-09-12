using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Common;
using Movieverse.Application.Interfaces;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Common;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Id;
using Movieverse.Infrastructure.Persistence;

namespace Movieverse.Infrastructure.Repositories;

public sealed class MediaRepository : IMediaRepository
{
	private readonly ILogger<MediaRepository> _logger;
	private readonly AppDbContext _dbContext;
	private readonly IMapper _mapper;
	
	public MediaRepository(ILogger<MediaRepository> logger, AppDbContext dbContext, IMapper mapper)
	{
		_logger = logger;
		_dbContext = dbContext;
		_mapper = mapper;
	}

	public async Task<Result<Media>> FindAsync(AggregateRootId id, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Finding media with id {id}...", id.ToString());
		
		var media = await _dbContext.Medias.FindAsync(new object?[] { id.Value }, cancellationToken).ConfigureAwait(false);
		return media is null ? Error.NotFound("Not found") : media;
	}

	public async Task<Result<List<Media>>> GetAllAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Getting all media");

		return await _dbContext.Medias
			.Include(x => x.AdvancedStatistics)
			.ThenInclude(x => x.Popularity)
			.ToListAsync(cancellationToken)
			.ConfigureAwait(false);
	}

	public async Task<bool> ExistsAsync(AggregateRootId id, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Checking if media with id {id} exists...", id.ToString());
		
		var media = await FindAsync(id, cancellationToken).ConfigureAwait(false);
		return media.IsSuccessful;
	}

	public async Task<bool> TitleExistsAsync(string title, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Checking if media with title {title} exists...", title);
		
		return await _dbContext.Medias.AnyAsync(m => m.Title == title, cancellationToken: cancellationToken).ConfigureAwait(false);
	}

	public async Task<Result<IPaginatedList<MediaInfoDto>>> FindMoviesByIdsAsync(List<AggregateRootId> ids, short? pageNumber, short? pageSize, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Getting movies with ids {ids}...", string.Join(", ", ids.Select(id => id.ToString())));
		
		var idsList = ids.Select(id => id.Value).ToList();
		
		return await _dbContext.Movies
			.Where(m => idsList.Contains(m.Id))
			.AsNoTracking()
			.ProjectToType<MediaInfoDto>()
			.ToPaginatedListAsync(pageNumber, pageSize)
			.ConfigureAwait(false);
	}

	public async Task<Result<IPaginatedList<MediaInfoDto>>> FindSeriesByIdsAsync(List<AggregateRootId> ids, short? pageNumber, short? pageSize, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Getting series with ids {ids}...", string.Join(", ", ids.Select(id => id.ToString())));
		
		var idsList = ids.Select(id => id.Value).ToList();
		
		return await _dbContext.Series
			.Where(s => idsList.Contains(s.Id))
			.AsNoTracking()
			.ProjectToType<MediaInfoDto>()
			.ToPaginatedListAsync(pageNumber, pageSize)
			.ConfigureAwait(false);
	}

	public async Task<Result> AddMovieAsync(Movie media, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Adding media with id {id}...", media.Id.ToString());
		
		await _dbContext.Movies.AddAsync(media, cancellationToken).ConfigureAwait(false);
		return Result.Ok();
	}
	
	public async Task<Result> AddSeriesAsync(Series media, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Adding media with id {id}...", media.Id.ToString());
		
		await _dbContext.Series.AddAsync(media, cancellationToken).ConfigureAwait(false);
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