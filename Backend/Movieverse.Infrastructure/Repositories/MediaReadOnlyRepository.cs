using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Common;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Common;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;
using Movieverse.Infrastructure.Persistence;

namespace Movieverse.Infrastructure.Repositories;

public sealed class MediaReadOnlyRepository : IMediaReadOnlyRepository
{
	private readonly ILogger<MediaReadOnlyRepository> _logger;
	private readonly ReadOnlyContext _dbContext;
	private readonly IMapper _mapper;
	
	public MediaReadOnlyRepository(ILogger<MediaReadOnlyRepository> logger, ReadOnlyContext dbContext, IMapper mapper)
	{
		_logger = logger;
		_dbContext = dbContext;
		_mapper = mapper;
	}

	public async Task<Result<MediaDto>> FindAsync(MediaId id, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Finding media with id {id}...", id.ToString());

		var media = await _dbContext.Medias
			.FirstOrDefaultAsync(m => m.Id == id, cancellationToken)
			.ConfigureAwait(false);
		
		return media switch
		{
			Movie movie => _mapper.Map<MovieDto>(movie),
			Series series => _mapper.Map<SeriesDto>(series),
			_ => Error.Invalid(MediaResources.MediaDoesNotExist)
		};
	}
	
	public async Task<Result<IPaginatedList<MediaDemoDto>>> GetUpcomingMediaAsync(PlatformId? platformId, short count, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Getting upcoming medias...");
		
		var medias = await _dbContext.Medias
			.AsNoTracking()
			.Where(m => m.Details.ReleaseDate > DateTime.UtcNow)
			.Where(m => platformId == null || m.PlatformIds.Any(p => p.Value == platformId.Value))
			.OrderBy(m => m.Details.ReleaseDate)
			.Take(count)
			.ProjectToType<MediaDemoDto>()
			.ToPaginatedListAsync(null, null, cancellationToken)
			.ConfigureAwait(false);
		
		return medias;
	}

	public async Task<Result<IPaginatedList<MediaDemoDto>>> GetLatestMediaAsync(PlatformId platformId, short? pageNumber = null, short? pageSize = null, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Getting upcoming series...");

		var medias = await _dbContext.Medias
			.AsNoTracking()
			.Where(m => m.Details.ReleaseDate <= DateTime.UtcNow)
			.Where(m =>  m.PlatformIds.Any(p => p.Value == platformId.Value))
			.OrderByDescending(m => m.Details.ReleaseDate)
			.ProjectToType<MediaDemoDto>()
			.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken)
			.ConfigureAwait(false);

		return medias;
	}

	
	public async Task<Result<IPaginatedList<MediaDemoDto>>> GetUpcomingMoviesAsync(PlatformId? platformId, short count, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Getting upcoming movies...");

		var movies = await _dbContext.Movies
			.AsNoTracking()
			.Where(m => m.Details.ReleaseDate > DateTime.UtcNow)
			.Where(m => platformId == null || m.PlatformIds.Any(p => p == platformId.Value))
			.OrderBy(m => m.Details.ReleaseDate)
			.Take(count)
			.ProjectToType<MediaDemoDto>()
			.ToPaginatedListAsync(null, null, cancellationToken)
			.ConfigureAwait(false);

		return movies;
	}

	public async Task<Result<IPaginatedList<MediaInfoDto>>> FindMoviesByIdsAsync(List<MediaId> ids, short? pageNumber, short? pageSize, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Getting movies with ids {ids}...", string.Join(", ", ids.Select(id => id.ToString())));
		
		var idsList = ids.Select(id => id.Value).ToList();
		
		return await _dbContext.Movies
			.Where(m => idsList.Contains(m.Id))
			.ProjectToType<MediaInfoDto>()
			.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken)
			.ConfigureAwait(false);
	}
	
	public async Task<Result<IPaginatedList<MediaInfoDto>>> FindSeriesByIdsAsync(List<MediaId> ids, short? pageNumber, short? pageSize, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Getting series with ids {ids}...", string.Join(", ", ids.Select(id => id.ToString())));
		
		var idsList = ids.Select(id => id.Value).ToList();
		
		return await _dbContext.Series
			.AsNoTracking()
			.Where(s => idsList.Contains(s.Id))
			.ProjectToType<MediaInfoDto>()
			.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken)
			.ConfigureAwait(false);
	}

	public async Task<Result<IPaginatedList<SearchMediaDto>>> SearchMediaAsync(string search, short? pageNumber, short? pageSize, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Searching media with string {Search}...", search);
		
		var medias = await _dbContext.Medias
			.AsNoTracking()
			.Where(m => m.Title.ToLower().StartsWith(search.ToLower()))
			.ProjectToType<SearchMediaDto>()
			.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken)
			.ConfigureAwait(false);

		return medias;
	}
}