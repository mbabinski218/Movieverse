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
		_logger.LogDebug("Database - Finding media with id {id}...", id.ToString());

		var media = await _dbContext.Medias
			.SingleOrDefaultAsync(m => m.Id == id, cancellationToken);
		
		return media switch
		{
			Movie movie => _mapper.Map<MovieDto>(movie),
			Series series => _mapper.Map<SeriesDto>(series),
			_ => Error.Invalid(MediaResources.MediaDoesNotExist)
		};
	}
	
	//TODO sprawdzić co jest szybsze
	public async Task<Result<IPaginatedList<MediaDemoDto>>> GetLatestMediaAsync(PlatformId platformId, short? pageNumber = null, short? pageSize = null, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting latest series...");

		var media = await _dbContext.Medias
			.AsNoTracking()
			.Where(m => m.Details.ReleaseDate <= DateTime.UtcNow)
			.Where(m => m.PlatformIds.Any(p => p.Value == platformId.Value))
			.OrderByDescending(m => m.Details.ReleaseDate)
			.ToListAsync(cancellationToken);
		
		var result = _mapper.Map<List<MediaDemoDto>>(media);
		
		return result.ToPaginatedList(pageNumber, pageSize);
		
		// var media = await _dbContext.Medias
		// 	.AsNoTracking()
		// 	.Where(m => m.Details.ReleaseDate <= DateTime.UtcNow)
		// 	.Where(m =>  m.PlatformIds.Any(p => p.Value == platformId.Value))
		// 	.OrderByDescending(m => m.Details.ReleaseDate)
		// 	.ProjectToType<MediaDemoDto>()
		// 	.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);
		//
		//  return media;
	}

	public async Task<Result<IPaginatedList<SearchMediaDto>>> FindUpcomingMoviesAsync(short? pageNumber, short? pageSize, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting upcoming movies...");

		var movies = await _dbContext.Movies
			.AsNoTracking()
			.Where(m => m.Details.ReleaseDate > DateTime.UtcNow)
			.OrderBy(m => m.Details.ReleaseDate)
			.ProjectToType<SearchMediaDto>()
			.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);

		return movies;
	}
	
	public async Task<Result<IPaginatedList<SearchMediaDto>>> FindUpcomingSeriesAsync(short? pageNumber, short? pageSize, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting upcoming media...");
		
		var series = await _dbContext.Series
			.AsNoTracking()
			.Where(s => s.Details.ReleaseDate > DateTime.UtcNow)
			.OrderBy(s => s.Details.ReleaseDate)
			.ProjectToType<SearchMediaDto>()
			.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);
		
		return series;
	}

	public async Task<Result<IPaginatedList<SearchMediaDto>>> FindTop100MoviesAsync(short? pageNumber, short? pageSize, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting top 100 movies...");
		
		var movies = await _dbContext.Movies
			.AsNoTracking()
			.Where(m => m.BasicStatistics.Rating > 0)
			.OrderByDescending(m => m.BasicStatistics.Rating)
			.ProjectToType<SearchMediaDto>()
			.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);

		return movies;
	}

	public async Task<Result<IPaginatedList<SearchMediaDto>>> FindTop100SeriesAsync(short? pageNumber, short? pageSize, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting top 100 series...");
		
		var series = await _dbContext.Series
			.AsNoTracking()
			.Where(s => s.BasicStatistics.Rating > 0)
			.OrderByDescending(s => s.BasicStatistics.Rating)
			.ProjectToType<SearchMediaDto>()
			.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);
		
		return series;
	}

	public Task<Result<IPaginatedList<SearchMediaDto>>> FindRecommendedMoviesAsync(short? pageNumber, short? pageSize, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	public Task<Result<IPaginatedList<SearchMediaDto>>> FindRecommendedSeriesAsync(short? pageNumber, short? pageSize, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	public async Task<Result<IEnumerable<ContentId>>> GetContentAsync(MediaId id, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting content for media id: ", id);

		var contentIds = await _dbContext.Medias
			.AsNoTracking()
			.Where(m => m.Id == id)
			.SelectMany(m => m.ContentIds)
			.ToListAsync(cancellationToken);

		return contentIds;
	}

	public async Task<Result<IPaginatedList<SearchMediaDto>>> FindMediaByIdsAsync(List<MediaId> ids, short? pageNumber, short? pageSize, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting media with ids {ids}...", string.Join(", ", ids.Select(id => id.ToString())));
		
		var idsList = ids.Select(id => id.Value).ToList();
		
		return await _dbContext.Medias
			.Where(m => idsList.Contains(m.Id))
			.OrderByDescending(x => x.Details.ReleaseDate)
			.ProjectToType<SearchMediaDto>()
			.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);
	}
	
	public async Task<Result<IPaginatedList<MediaInfoDto>>> FindMoviesByIdsAsync(List<MediaId> ids, short? pageNumber, short? pageSize, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting movies with ids {ids}...", string.Join(", ", ids.Select(id => id.ToString())));
		
		var idsList = ids.Select(id => id.Value).ToList();
		
		return await _dbContext.Movies
			.Where(m => idsList.Contains(m.Id))
			.ProjectToType<MediaInfoDto>()
			.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);
	}
	
	public async Task<Result<IPaginatedList<MediaInfoDto>>> FindSeriesByIdsAsync(List<MediaId> ids, short? pageNumber, short? pageSize, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting series with ids {ids}...", string.Join(", ", ids.Select(id => id.ToString())));
		
		var idsList = ids.Select(id => id.Value).ToList();
		
		return await _dbContext.Series
			.AsNoTracking()
			.Where(s => idsList.Contains(s.Id))
			.ProjectToType<MediaInfoDto>()
			.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);
	}

	public async Task<Result<IPaginatedList<SearchMediaDto>>> SearchMediaAsync(string search, short? pageNumber, short? pageSize, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Searching media with string {Search}...", search);
		
		var media = await _dbContext.Medias
			.AsNoTracking()
			.Where(m => m.Title.ToLower().StartsWith(search.ToLower()))
			.ProjectToType<SearchMediaDto>()
			.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);

		return media;
	}
	
	public async Task<Result<IPaginatedList<SearchMediaDto>>> FindMostPopularMoviesAsync(short? pageNumber, short? pageSize, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting most popular movies...");

		var media = await _dbContext.Movies
			.AsNoTracking()
            .Where(m => m.CurrentPosition <= 100)
			.OrderBy(m => m.CurrentPosition)
			.ProjectToType<SearchMediaDto>()
			.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);

		return media;
	}
	
	public async Task<Result<IPaginatedList<SearchMediaDto>>> FindMostPopularSeriesAsync(short? pageNumber, short? pageSize, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting most popular series...");
		
		var media = await _dbContext.Series
			.AsNoTracking()
			.Where(s => s.CurrentPosition <= 100)
			.OrderBy(s => s.CurrentPosition)
			.ProjectToType<SearchMediaDto>()
			.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);

		return media;
	}
	
	public async Task<Result<IPaginatedList<SearchMediaDto>>> FindMostPopularAsync(string? type, short? pageNumber, short? pageSize, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting top 100 series...");
		
		var media = await _dbContext.Medias
			.AsNoTracking()
			.Where(x => type == null || type == nameof(Movie) ? x is Movie : x is Series)
			.Where(m => m.CurrentPosition <= 100)
			.OrderBy(m => m.CurrentPosition)
			.ProjectToType<SearchMediaDto>()
			.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);

		return media;
	}
	
	public async Task<Result<IPaginatedList<SearchMediaDto>>> SearchMoviesWithFiltersAsync(string? search, GenreId? genre, 
		short? pageNumber, short? pageSize, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Searching movies with string {Search}...", search);
		
		var media = await _dbContext.Movies
			.AsNoTracking()
			.Where(m => search == null || m.Title.ToLower().StartsWith(search.ToLower()))
			.Where(m => genre == null || m.GenreIds.Any(g => g.Value == genre.Value))
			.ProjectToType<SearchMediaDto>()
			.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);

		return media;
	}
	
	public async Task<Result<IPaginatedList<SearchMediaDto>>> SearchSeriesWithFiltersAsync(string? search, GenreId? genre, short? pageNumber, short? pageSize, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Searching series with string {Search}...", search);
		
		var media = await _dbContext.Series
			.AsNoTracking()
			.Where(s => search == null || s.Title.ToLower().StartsWith(search.ToLower()))
			.Where(s => genre == null || s.GenreIds.Any(g => g.Value == genre.Value))
			.ProjectToType<SearchMediaDto>()
			.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);

		return media;
	}
}