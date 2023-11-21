using Movieverse.Contracts.DataTransferObjects.Platform;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Application.Interfaces.Repositories;

public interface IPlatformReadOnlyRepository
{
	Task<Result<PlatformDto>> FindAsync(PlatformId id, CancellationToken cancellationToken = default);
	Task<Result<IEnumerable<Platform>>> GetAllAsync(CancellationToken cancellationToken = default);
	Task<Result<IEnumerable<MediaId>>> GetAllMediaIdsAsync(PlatformId id, CancellationToken cancellationToken = default);
	Task<Result<IEnumerable<PlatformInfoDto>>> GetPlatformsInfoAsync(IEnumerable<PlatformId> ids, CancellationToken cancellationToken = default);
	Task<Result<IEnumerable<PlatformDemoDto>>> GetPlatformsDemoAsync(CancellationToken cancellationToken = default);
}