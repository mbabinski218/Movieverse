using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;
using Movieverse.Infrastructure.Persistence;

namespace Movieverse.Infrastructure.Repositories;

public sealed class ContentReadOnlyRepository : IContentReadOnlyRepository
{
	private readonly ILogger<ContentReadOnlyRepository> _logger;
	private readonly ReadOnlyContext _dbContext;

	public ContentReadOnlyRepository(ILogger<ContentReadOnlyRepository> logger, ReadOnlyContext dbContext)
	{
		_logger = logger;
		_dbContext = dbContext;
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
		await _dbContext.Contents
			.FirstOrDefaultAsync(c => c.Id == id, cancellationToken)
			;
}