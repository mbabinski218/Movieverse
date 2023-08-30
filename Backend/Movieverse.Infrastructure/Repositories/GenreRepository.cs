using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Common;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Resources;
using Movieverse.Contracts.DataTransferObjects.Genre;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Id;
using Movieverse.Infrastructure.Persistence;

namespace Movieverse.Infrastructure.Repositories;

public sealed class GenreRepository : IGenreRepository
{
	private readonly ILogger<GenreRepository> _logger;
	private readonly AppDbContext _dbContext;
	private readonly IMapper _mapper;

	public GenreRepository(ILogger<GenreRepository> logger, AppDbContext dbContext, IMapper mapper)
	{
		_logger = logger;
		_dbContext = dbContext;
		_mapper = mapper;
	}

	public async Task<Result> AddAsync(Genre genre, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Adding genre with id {Id} to database", genre.Id.ToString());
		
		await _dbContext.Genres.AddAsync(genre, cancellationToken).ConfigureAwait(false);
		return Result.Ok();
	}

	public async Task<Result<Genre>> FindByIdAsync(AggregateRootId id, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Finding genre with id {Id} from database", id);
		
		var genre = await _dbContext.Genres.FindAsync(new object?[] { id.Value }, cancellationToken).ConfigureAwait(false);
		return genre is null ? Error.NotFound(GenreResources.GenreNotFound) : genre;
	}
	
	public async Task<Result<IPaginatedList<GenreDto>>> GetAllAsync(short? pageNumber = null, short? pageSize = null, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Getting all genres from database");
		
		return await _dbContext.Genres
			.AsNoTracking()
			.ProjectToType<GenreDto>()
			.ToPaginatedListAsync(pageNumber, pageSize)
			.ConfigureAwait(false);
	}
}