﻿using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Resources;
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

	public async Task<Result<Content>> FindAsync(AggregateRootId id, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Finding content with id {id}...", id.ToString());
		
		var content = await _dbContext.Contents.FindAsync(new object?[] { id.Value }, cancellationToken).ConfigureAwait(false);
		return content is null ? Error.NotFound(ContentResources.ContentNotFound) : content;
	}

	public async Task<Result> UpdateAsync(Content content, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Updating content with id {id}...", content.Id.ToString());

		_dbContext.Contents.Update(content);
		return await Task.FromResult(Result.Ok());
	}

	public async Task<Result<bool>> ExistsAsync(AggregateRootId id, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Checking if content with id {Id} exists", id);
		
		var image = await FindByIdAsync(id, cancellationToken);
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
		
		var image = await FindByIdAsync(id, cancellationToken);
		return image is not null ? image.ContentType : Error.NotFound(ContentResources.ContentNotFound);
	}

	public async Task<Result<string>> GetPathAsync(AggregateRootId id, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Getting path for content with id {Id}", id);

		var image = await FindByIdAsync(id, cancellationToken);
		return image is not null ? image.Path : Error.NotFound(ContentResources.ContentNotFound);
	}
	
	private async Task<Content?> FindByIdAsync(AggregateRootId id, CancellationToken cancellationToken) =>
		await _dbContext.Contents.FindAsync(new object?[] { id.Value }, cancellationToken);
}