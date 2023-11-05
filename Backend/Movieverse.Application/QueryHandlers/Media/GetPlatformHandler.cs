using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.DataTransferObjects.Platform;
using Movieverse.Contracts.Queries.Media;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.QueryHandlers.Media;

public sealed class GetPlatformHandler : IRequestHandler<GetPlatformQuery, Result<IEnumerable<PlatformInfoDto>>>
{
	private readonly ILogger<GetPlatformHandler> _logger;
	private readonly IPlatformReadOnlyRepository _platformRepository;
	private readonly IMediaReadOnlyRepository _mediaRepository;

	public GetPlatformHandler(ILogger<GetPlatformHandler> logger, IPlatformReadOnlyRepository platformRepository, IMediaReadOnlyRepository mediaRepository)
	{
		_logger = logger;
		_platformRepository = platformRepository;
		_mediaRepository = mediaRepository;
	}

	public async Task<Result<IEnumerable<PlatformInfoDto>>> Handle(GetPlatformQuery request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Retrieving platforms for media with id: {Id}", request.Id);
		
		var platformIds = await _mediaRepository.GetPlatformIdsAsync(request.Id, cancellationToken);
		if (platformIds.IsUnsuccessful)
		{
			return platformIds.Error;
		}
		
		return await _platformRepository.GetPlatformsInfoAsync(platformIds.Value, cancellationToken);
	}
}