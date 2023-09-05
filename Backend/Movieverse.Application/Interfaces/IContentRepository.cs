using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.Application.Interfaces;

public interface IContentRepository
{
	Task<Result<Content>> FindAsync(AggregateRootId id, CancellationToken cancellationToken = default);
	Task<Result> UpdateAsync(Content content, CancellationToken cancellationToken = default);
	Task<Result<bool>> ExistsAsync(AggregateRootId id, CancellationToken cancellationToken = default);
	Task<Result> AddAsync(Content content, CancellationToken cancellationToken = default);
	Task<Result<string>> GetContentTypeAsync(AggregateRootId id, CancellationToken cancellationToken = default);
	Task<Result<string>> GetPathAsync(AggregateRootId id, CancellationToken cancellationToken = default);
}