using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Application.Interfaces.Repositories;

public interface IPlatformRepository
{
	Task<Result> AddAsync(Platform platform, CancellationToken cancellationToken = default);
	Task<Result> UpdateAsync(Platform platform, CancellationToken cancellationToken = default);
	Task<Result<Platform>> FindAsync(PlatformId id, CancellationToken cancellationToken = default);
	Task<Result<IEnumerable<Platform>>> GetAllAsync(CancellationToken cancellationToken = default);
	Task<Result<List<MediaId>>> GetAllMediaIdsAsync(PlatformId id, CancellationToken cancellationToken = default);
}