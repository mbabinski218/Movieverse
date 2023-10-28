using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.DataTransferObjects.Genre;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;
using Movieverse.Infrastructure.Persistence;

namespace Movieverse.Infrastructure.Repositories;

public sealed class GenreReadOnlyRepository : IGenreReadOnlyRepository
{
	private readonly ILogger<GenreReadOnlyRepository> _logger;
	private readonly ReadOnlyContext _dbContext;
	private readonly IMapper _mapper;

	public GenreReadOnlyRepository(ILogger<GenreReadOnlyRepository> logger, ReadOnlyContext dbContext, IMapper mapper)
	{
		_logger = logger;
		_dbContext = dbContext;
		_mapper = mapper;
	}
	
	public async Task<Result<GenreDto>> FindAsync(GenreId id, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Finding genre with id {Id} from database", id);
		
		var genre = await _dbContext.Genres
			.SingleOrDefaultAsync(g => g.Id == id, cancellationToken);
		
		return genre is null ? Error.NotFound(GenreResources.GenreNotFound) : _mapper.Map<GenreDto>(genre);
	}
	
	public async Task<Result<IEnumerable<GenreDto>>> GetAllAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting all genres from database");
		
		return await _dbContext.Genres
			.AsNoTracking()
			.ProjectToType<GenreDto>()
			.ToListAsync(cancellationToken);
	}
}