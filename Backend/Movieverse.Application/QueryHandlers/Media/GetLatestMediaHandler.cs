using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Contracts.Queries.Media;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Application.QueryHandlers.Media;

public sealed class GetLatestMediaHandler : IRequestHandler<GetLatestMediaQuery, Result<IEnumerable<FilteredMediaDto>>>
{
	private readonly ILogger<GetUpcomingMediaHandler> _logger;
	private readonly IMediaReadOnlyRepository _mediaRepository;
	private readonly IPlatformReadOnlyRepository _platformRepository;

	public GetLatestMediaHandler(ILogger<GetUpcomingMediaHandler> logger, IMediaReadOnlyRepository mediaRepository, IPlatformReadOnlyRepository platformRepository)
	{
		_logger = logger;
		_mediaRepository = mediaRepository;
		_platformRepository = platformRepository;
	}

	public async Task<Result<IEnumerable<FilteredMediaDto>>> Handle(GetLatestMediaQuery request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Getting latest media...");

		return request.PlatformId is null
			? await GetAllLatestMediaAsync(request.PageNumber, request.PageSize).ConfigureAwait(false) 
			: await GetLatestMediaByPlaceAsync(request.PlatformId, request.PageNumber, request.PageSize).ConfigureAwait(false);
	}
	
	private async Task<Result<IEnumerable<FilteredMediaDto>>> GetAllLatestMediaAsync(short? pageNumber, short? pageSize)
	{
		var platforms = await _platformRepository.GetAllAsync().ConfigureAwait(false);
		if (platforms.IsUnsuccessful)
		{
			return platforms.Error;
		}

		var latestMedia = new List<FilteredMediaDto>();
		
		foreach (var platform in platforms.Value)
		{
			var latestMediaResult = await _mediaRepository.GetLatestMediaAsync(platform.Id.Value, pageNumber, pageSize).ConfigureAwait(false);
			if (latestMediaResult.IsUnsuccessful)
			{
				return latestMediaResult.Error;
			}

			latestMedia.Add(new FilteredMediaDto
			{
				PlatformId = platform.Id,
				PlatformName = platform.Name,
				Media = latestMediaResult.Value
			});
		}

		return latestMedia;
	}
	
	private async Task<Result<IEnumerable<FilteredMediaDto>>> GetLatestMediaByPlaceAsync(PlatformId platformId, short? pageNumber, short? pageSize)
	{
		var platform = await _platformRepository.FindAsync(platformId).ConfigureAwait(false);
		if (platform.IsUnsuccessful)
		{
			return platform.Error;
		}
		
		var latestMediaResult = await _mediaRepository.GetLatestMediaAsync(platform.Value.Id, pageNumber, pageSize).ConfigureAwait(false);
		if (latestMediaResult.IsUnsuccessful)
		{
			return latestMediaResult.Error;
		}

		return new List<FilteredMediaDto>
		{
			new()
			{
				PlatformId = platformId,
				PlatformName = platform.Value.Name,
				Media = latestMediaResult.Value
			}
		};
	}
}