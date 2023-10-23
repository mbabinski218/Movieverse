using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Common.Extensions;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.DataTransferObjects.Platform;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;
using Movieverse.Infrastructure.Persistence;

namespace Movieverse.Infrastructure.Repositories;

public sealed class PlatformReadOnlyRepository : IPlatformReadOnlyRepository
{
	private readonly ILogger<PlatformReadOnlyRepository> _logger;
	private readonly ReadOnlyContext _dbContext;
	private readonly IMapper _mapper;

	public PlatformReadOnlyRepository(ILogger<PlatformReadOnlyRepository> logger, ReadOnlyContext dbContext, IMapper mapper)
	{
		_logger = logger;
		_dbContext = dbContext;
		_mapper = mapper;
	}

	public async Task<Result<PlatformDto>> FindAsync(PlatformId id, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting platform with id {id}...", id.ToString());
		
		var platform = await _dbContext.Platforms
			.AsNoTracking()
			.Include(p => p.LogoId)
			.SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
		
		return platform is null ? Error.NotFound(PlatformResources.PlatformDoesNotExist) : new PlatformDto
		{
			Id = platform.Id,
			Name = platform.Name,
			LogoId = platform.LogoId.GetValue(),
			Price = platform.Price
		};
	}

	public async Task<Result<IEnumerable<Platform>>> GetAllAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting all platforms...");

		return await _dbContext.Platforms
			.AsNoTracking()
			.ToListAsync(cancellationToken);
	}

	public async Task<Result<IEnumerable<MediaId>>> GetAllMediaIdsAsync(PlatformId id, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Getting all media ids for platform with id {id}...", id.ToString());
		
		return await _dbContext.Platforms
			.AsNoTracking()
			.Where(p => p.Id == id.Value)
			.SelectMany(p => p.MediaIds)
			.ToListAsync(cancellationToken);
	}
}