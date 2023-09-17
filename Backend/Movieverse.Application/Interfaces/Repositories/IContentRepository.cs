using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Application.Interfaces.Repositories;

public interface IContentRepository
{
	Task<Result<Content>> FindAsync(ContentId id, CancellationToken cancellationToken = default);
	Task<Result> UpdateAsync(Content content, CancellationToken cancellationToken = default);
	Task<Result<bool>> ExistsAsync(ContentId id, CancellationToken cancellationToken = default);
	Task<Result> AddAsync(Content content, CancellationToken cancellationToken = default);
	Task<Result<string>> GetContentTypeAsync(ContentId id, CancellationToken cancellationToken = default);
	Task<Result<string>> GetPathAsync(ContentId id, CancellationToken cancellationToken = default);
}