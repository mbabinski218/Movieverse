using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.Application.Interfaces;

public interface IContentRepository
{
	Task<Result<bool>> ExistsAsync(AggregateRootId id, CancellationToken cancellationToken);
	Task<Result> AddAsync(Content content, CancellationToken cancellationToken);
	Task<Result<string>> GetContentTypeAsync(AggregateRootId id, CancellationToken cancellationToken);
	Task<Result<string>> GetPathAsync(AggregateRootId id, CancellationToken cancellationToken);
}