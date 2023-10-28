using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;
using Movieverse.Infrastructure.Persistence;

namespace Movieverse.Infrastructure.Repositories;

public sealed class ContentRepository : IContentRepository
{
	private readonly ILogger<ContentRepository> _logger;
	private readonly Context _dbContext;

	public ContentRepository(ILogger<ContentRepository> logger, Context dbContext)
	{
		_logger = logger;
		_dbContext = dbContext;
	}

	public async Task<Result<Content>> FindAsync(ContentId id, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Finding content with id {id}...", id.ToString());
		
		var content = await _dbContext.Contents
			.SingleOrDefaultAsync(c => c.Id == id, cancellationToken);
		
		return content is null ? Error.NotFound(ContentResources.ContentNotFound) : content;
	}

	public async Task<Result> UpdateAsync(Content content, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Updating content with id {id}...", content.Id.ToString());

		_dbContext.Contents.Update(content);
		return await Task.FromResult(Result.Ok());
	}

	public async Task<Result<bool>> ExistsAsync(ContentId id, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Checking if content with id {Id} exists", id);
		
		var image = await FindByIdAsync(id, cancellationToken);
		return image is not null;
	}

	public async Task<Result> AddAsync(Content content, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Adding content with id {Id}", content.Id);
		
		await _dbContext.Contents.AddAsync(content, cancellationToken);
		return Result.Ok();
	}

	public async Task<Result<string>> GetContentTypeAsync(ContentId id, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Database - Getting content type for content with id {Id}", id);
		
		var image = await FindByIdAsync(id, cancellationToken);
		return image is not null ? image.ContentType : Error.NotFound(ContentResources.ContentNotFound);
	}

	public async Task<Result<string>> GetPathAsync(ContentId id, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Database - Getting path for content with id {Id}", id);

		var image = await FindByIdAsync(id, cancellationToken);
		return image is not null ? image.Path : Error.NotFound(ContentResources.ContentNotFound);
	}
	
	private async Task<Content?> FindByIdAsync(ContentId id, CancellationToken cancellationToken) =>
		await _dbContext.Contents.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
}