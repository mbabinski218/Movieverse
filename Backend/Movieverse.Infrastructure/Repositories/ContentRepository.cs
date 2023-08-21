using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Id;
using Movieverse.Infrastructure.Persistence;

namespace Movieverse.Infrastructure.Repositories;

public sealed class ContentRepository : IContentRepository
{
	private readonly ILogger<ContentRepository> _logger;
	private readonly AppDbContext _dbContext;

	public ContentRepository(ILogger<ContentRepository> logger, AppDbContext dbContext)
	{
		_logger = logger;
		_dbContext = dbContext;
	}

	public async Task<Result<bool>> ExistsAsync(AggregateRootId id, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Checking if content with id {Id} exists", id);
		
		var image = await FindAsync(id, cancellationToken);
		return image is not null;
	}

	public async Task<Result> AddAsync(Content content, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Adding content with id {Id}", content.Id);
		
		await _dbContext.Contents.AddAsync(content, cancellationToken);
		return Result.Ok();
	}

	public async Task<Result<string>> GetContentTypeAsync(AggregateRootId id, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Getting content type for content with id {Id}", id);
		
		var image = await FindAsync(id, cancellationToken);
		return image is not null ? image.ContentType : Error.NotFound("Content not found.");
	}

	public async Task<Result<string>> GetPathAsync(AggregateRootId id, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Getting path for content with id {Id}", id);

		var image = await FindAsync(id, cancellationToken);
		return image is not null ? image.Path : Error.NotFound("Content not found.");
	}
	
	private async Task<Content?> FindAsync(AggregateRootId id, CancellationToken cancellationToken) =>
		await _dbContext.Contents.FindAsync(new object?[] { id.Value }, cancellationToken);
}