using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Resources;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;
using Movieverse.Infrastructure.Persistence;

namespace Movieverse.Infrastructure.Repositories;

public sealed class PlatformRepository : IPlatformRepository
{
	private readonly ILogger<PlatformRepository> _logger;
	private readonly AppDbContext _dbContext;

	public PlatformRepository(ILogger<PlatformRepository> logger, AppDbContext dbContext)
	{
		_logger = logger;
		_dbContext = dbContext;
	}

	public async Task<Result<Platform>> FindAsync(PlatformId id, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Getting platform with id {id}...", id.ToString());
		
		var platform = await _dbContext.Platforms.FindAsync(new object?[] { id.Value }, cancellationToken).ConfigureAwait(false);
		return platform is null ? Error.NotFound(PlatformResources.PlatformDoesNotExist) : platform;
	}

	public async Task<Result<IEnumerable<Platform>>> GetAllAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Getting all platforms...");

		return await _dbContext.Platforms.AsNoTracking().ToListAsync(cancellationToken).ConfigureAwait(false);
	}

	public async Task<Result<List<MediaId>>> GetAllMediaIdsAsync(PlatformId id, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Getting all media ids for platform with id {id}...", id.ToString());
		
		return await _dbContext.Platforms
			.Where(p => p.Id == id.Value)
			.Include(p => p.MediaIds)
			.SelectMany(p => p.MediaIds)
			.AsNoTracking()
			.ToListAsync(cancellationToken);
	}

	public async Task<Result> AddAsync(Platform platform, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Adding platform with id {id}...", platform.Id.ToString());
		
		await _dbContext.Platforms.AddAsync(platform, cancellationToken).ConfigureAwait(false);
		return Result.Ok();
	}

	public async Task<Result> UpdateAsync(Platform platform, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Updating platform with id {id}...", platform.Id.ToString());
		
		_dbContext.Platforms.Update(platform);
		return await Task.FromResult(Result.Ok());
	}
}