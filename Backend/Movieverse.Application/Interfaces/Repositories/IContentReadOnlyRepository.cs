using Movieverse.Contracts.DataTransferObjects.Content;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Application.Interfaces.Repositories;

public interface IContentReadOnlyRepository
{
	Task<Result<string>> GetContentTypeAsync(ContentId id, CancellationToken cancellationToken = default);
	Task<Result<string>> GetPathAsync(ContentId id, CancellationToken cancellationToken = default);
	Task<Result<IEnumerable<ContentInfoDto>>> GetPathsAsync(IEnumerable<ContentId> ids, CancellationToken cancellationToken = default);
}