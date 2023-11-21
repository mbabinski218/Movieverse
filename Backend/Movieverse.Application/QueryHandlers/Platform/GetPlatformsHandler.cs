using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Contracts.DataTransferObjects.Platform;
using Movieverse.Contracts.Queries.Platform;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.QueryHandlers.Platform;

public sealed class GetPlatformsHandler : IRequestHandler<GetPlatformsQuery, Result<IEnumerable<PlatformDemoDto>>>
{
	private readonly ILogger<GetPlatformsHandler> _logger;
	private readonly IPlatformReadOnlyRepository _platformRepository;

	public GetPlatformsHandler(ILogger<GetPlatformsHandler> logger, IPlatformReadOnlyRepository platformRepository)
	{
		_logger = logger;
		_platformRepository = platformRepository;
	}

	public async Task<Result<IEnumerable<PlatformDemoDto>>> Handle(GetPlatformsQuery request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("GetPlatformsHandler.Handle - Retrieving platforms");

		return await _platformRepository.GetPlatformsDemoAsync(cancellationToken);
	}
}