using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Common;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.DataTransferObjects.Genre;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Common;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.Entities;
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

		var media = await _dbContext.Media
			.SingleOrDefaultAsync(m => m.Id == id, cancellationToken);
		
		return media switch
		{
			Movie movie => _mapper.Map<MovieDto>(movie),
			Series series => _mapper.Map<SeriesDto>(series),
			_ => Error.Invalid(MediaResources.MediaDoesNotExist)
		};
	}
	
	public async Task<Result<IPaginatedList<MediaDemoDto>>> GetLatestMediaAsync(PlatformId platformId, short? pageNumber = null, short? pageSize = null, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting latest series...");

		var media = await _dbContext.Media
			.AsNoTracking()
			.Where(m => m.Details.ReleaseDate <= DateTime.UtcNow)
			.Where(m => m.PlatformIds.Any(p => p.Value == platformId.Value))
			.OrderByDescending(m => m.Details.ReleaseDate)
			.ToListAsync(cancellationToken);
		
		var result = _mapper.Map<List<MediaDemoDto>>(media);
		
		return result.ToPaginatedList(pageNumber, pageSize);
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

	public async Task<Result<IEnumerable<GenreDto>>> GetAllGenresAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting all genres...");
		
		var genres = await _dbContext.Genres
			.AsNoTracking()
			.ProjectToType<GenreDto>()
			.ToListAsync(cancellationToken);

		return genres;
	}

	public async Task<Result<IEnumerable<ContentId>>> GetContentAsync(MediaId id, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting content for media id: {id}", id.ToString());

		var contentIds = await _dbContext.Media
			.AsNoTracking()
			.Where(m => m.Id == id)
			.SelectMany(m => m.ContentIds)
			.ToListAsync(cancellationToken);

		return contentIds;
	}

	public async Task<Result<IEnumerable<PlatformId>>> GetPlatformIdsAsync(MediaId id, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting platforms for media id: {id}", id.ToString());
		
		var platformIds = await _dbContext.Media
			.AsNoTracking()
			.Where(m => m.Id == id)
			.SelectMany(m => m.PlatformIds)
			.ToListAsync(cancellationToken);

		return platformIds;
	}

	public async Task<Result<IEnumerable<GenreInfoDto>>> GetGenresAsync(MediaId id, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting genres for media id: {id}", id.ToString());
		
		var genreIds = await _dbContext.Media
			.AsNoTracking()
			.Where(m => m.Id == id)
			.SelectMany(m => m.Genres)
			// .Select(mg => mg.Genre)
			.ProjectToType<GenreInfoDto>()
			.ToListAsync(cancellationToken);

		return genreIds;
	}

	public async Task<Result<IEnumerable<Staff>>> GetStaffAsync(MediaId id, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting staff for media id: {id}", id.ToString());
		
		var staff = await _dbContext.Media
			.AsNoTracking()
			.Where(m => m.Id == id)
			.SelectMany(m => m.Staff)
			.ToListAsync(cancellationToken);

		return staff;
	}

	public async Task<Result<StatisticsDto>> GetStatisticsAsync(MediaId id, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting statistics for media id: {id}", id.ToString());
		
		var statistics = await _dbContext.Media
			.AsNoTracking()
			.Where(m => m.Id == id)
			.Select(m => new { m.BasicStatistics.OnWatchlistCount, m.AdvancedStatistics })
			.SingleOrDefaultAsync(cancellationToken);

		return statistics is null
			? Error.NotFound(MediaResources.MediaDoesNotExist)
			: new StatisticsDto
			{
				OnWatchlistCount = statistics.OnWatchlistCount,
				BoxOffice = statistics.AdvancedStatistics.BoxOffice,
				Popularity = _mapper.Map<List<PopularityDto>>(statistics.AdvancedStatistics.Popularity)
			};
	}

	public async Task<Result<SeasonInfoDto>> GetSeasonsAsync(MediaId id, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting seasons for media id: {id}", id.ToString());
		
		var info = await _dbContext.Series
			.AsNoTracking()
			.Where(s => s.Id == id)
			.ProjectToType<SeasonInfoDto>()
			.SingleOrDefaultAsync(cancellationToken);

		if (info is null)
		{
			return Error.NotFound(MediaResources.MediaIsNotSeries);
		}
			
		info.Seasons.ForEach(e =>
		{
			e.Episodes = e.Episodes.OrderBy(x => x.EpisodeNumber).ToList();
		});

		info.Seasons = info.Seasons.OrderBy(x => x.SeasonNumber).ToList();

		return info;
	}

	public async Task<Result<IEnumerable<MediaSectionDto>>> GetMediaSectionAsync(IEnumerable<MediaId> ids, CancellationToken cancellationToken = default)
	{
		var idsList = ids.Select(id => id.Value).ToList();
		
		_logger.LogDebug("Database - Getting media section for media ids: {ids}", string.Join(", ", idsList.Select(id => id.ToString())));
		
		var media = await _dbContext.Media
			.AsNoTracking()
			.Where(m => idsList.Contains(m.Id))
			.ProjectToType<MediaSectionDto>()
			.ToListAsync(cancellationToken);
		
		return media.OrderByDescending(m => m.Rating).ToList();
	}

	public async Task<Result<IEnumerable<ReviewDto>>> GetReviewsAsync(MediaId id, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting reviews for media id: {id}", id.ToString());

		var reviews = await _dbContext.Media
			.AsNoTracking()
			.Where(m => m.Id == id)
			.SelectMany(m => m.Reviews)
			.Where(r => !r.Banned)
			.ProjectToType<ReviewDto>()
			.OrderByDescending(r => r.Date)
			.ToListAsync(cancellationToken);

		return reviews;
	}

	public async Task<Result<IPaginatedList<SearchMediaDto>>> FindMediaByIdsAsync(List<MediaId> ids, short? pageNumber, short? pageSize, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting media with ids {ids}...", string.Join(", ", ids.Select(id => id.ToString())));
		
		var idsList = ids.Select(id => id.Value).ToList();
		
		return await _dbContext.Media
			.AsNoTracking()
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
			.AsNoTracking()
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
		
		var media = await _dbContext.Media
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
		
		var media = await _dbContext.Media
			.AsNoTracking()
			.Where(x => type == null || type == nameof(Movie) ? x is Movie : x is Series)
			.Where(m => m.CurrentPosition <= 100)
			.OrderBy(m => m.CurrentPosition)
			.ProjectToType<SearchMediaDto>()
			.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);

		return media;
	}
	
	public async Task<Result<IPaginatedList<SearchMediaDto>>> SearchMoviesWithFiltersAsync(string? search, int? genre, 
		short? pageNumber, short? pageSize, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Searching movies with string {Search}...", search);
		
		var media = await _dbContext.Movies
			.AsNoTracking()
			.Where(m => search == null || m.Title.ToLower().StartsWith(search.ToLower()))
			// .Where(m => genre == null || m.Genres.Any(mg => mg.GenreId == genre.Value))
			.Where(m => genre == null || m.Genres.Any(g => g.Id == genre.Value))
			.ProjectToType<SearchMediaDto>()
			.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);

		return media;
	}
	
	public async Task<Result<IPaginatedList<SearchMediaDto>>> SearchSeriesWithFiltersAsync(string? search, int? genre, short? pageNumber, short? pageSize, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Searching series with string {Search}...", search);
		
		var media = await _dbContext.Series
			.AsNoTracking()
			.Where(s => search == null || s.Title.ToLower().StartsWith(search.ToLower()))
			// .Where(s => genre == null || s.Genres.Any(mg => mg.GenreId == genre.Value))
			.Where(s => genre == null || s.Genres.Any(g => g.Id == genre.Value))
			.ProjectToType<SearchMediaDto>()
			.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);

		return media;
	}
}