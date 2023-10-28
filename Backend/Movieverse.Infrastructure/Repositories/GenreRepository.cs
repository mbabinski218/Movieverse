using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Common;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.DataTransferObjects.Genre;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;
using Movieverse.Infrastructure.Persistence;

namespace Movieverse.Infrastructure.Repositories;

public sealed class GenreRepository : IGenreRepository
{
	private readonly ILogger<GenreRepository> _logger;
	private readonly Context _dbContext;

	public GenreRepository(ILogger<GenreRepository> logger, Context dbContext)
	{
		_logger = logger;
		_dbContext = dbContext;
	}

	public async Task<Result<Genre>> FindAsync(GenreId id, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Finding genre with id {Id} from database", id);
		
		var genre = await _dbContext.Genres
			.SingleOrDefaultAsync(g => g.Id == id, cancellationToken);
		
		return genre is null ? Error.NotFound(GenreResources.GenreNotFound) : genre;
	}
	
	public async Task<Result> AddAsync(Genre genre, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Adding genre with id {Id} to database", genre.Id.ToString());
		
		await _dbContext.Genres.AddAsync(genre, cancellationToken);
		return Result.Ok();
	}
	
	public async Task<Result<IPaginatedList<GenreDto>>> GetAllAsync(short? pageNumber = null, short? pageSize = null, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting all genres from database");
		
		return await _dbContext.Genres
			.AsNoTracking()
			.ProjectToType<GenreDto>()
			.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);
	}
}