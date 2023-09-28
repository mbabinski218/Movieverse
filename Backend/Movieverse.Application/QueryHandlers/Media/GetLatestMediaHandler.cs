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

		return request.PlaceId is null || request.PlaceName is null
			? await GetAllLatestMediaAsync(request.PageNumber, request.PageSize).ConfigureAwait(false) 
			: await GetLatestMediaByPlaceAsync(request.PlaceName, request.PlaceId, request.PageNumber, request.PageSize).ConfigureAwait(false);
	}
	
	private async Task<Result<IEnumerable<FilteredMediaDto>>> GetAllLatestMediaAsync(short? pageNumber, short? pageSize)
	{
		var platforms = await _platformRepository.GetAllAsync().ConfigureAwait(false);
		if (platforms.IsUnsuccessful)
		{
			return platforms.Error;
		}

		var latestMedia = new List<FilteredMediaDto>();
		
		var latestMediaInTheaters = await _mediaRepository.GetLatestMediaAsync(null, pageNumber, pageSize).ConfigureAwait(false);
		if (latestMediaInTheaters.IsUnsuccessful)
		{
			return latestMediaInTheaters.Error;
		}
		latestMedia.Add(new FilteredMediaDto
		{
			PlaceName = "Theaters",
			PlaceId = null,
			Medias = latestMediaInTheaters.Value
		});
		
		foreach (var platform in platforms.Value)
		{
			var latestMediaResult = await _mediaRepository.GetLatestMediaAsync(PlatformId.Create(platform.Id), pageNumber, pageSize).ConfigureAwait(false);
			if (latestMediaResult.IsUnsuccessful)
			{
				return latestMediaResult.Error;
			}

			latestMedia.Add(new FilteredMediaDto
			{
				PlaceName = platform.Name,
				PlaceId = platform.Id,
				Medias = latestMediaResult.Value
			});
		}

		return latestMedia;
	}
	
	private async Task<Result<IEnumerable<FilteredMediaDto>>> GetLatestMediaByPlaceAsync(string placeName, PlatformId placeId, short? pageNumber, short? pageSize)
	{
		if (placeName == "Theaters")
		{
			var latestMediaInTheaters = await _mediaRepository.GetLatestMediaAsync(null, pageNumber, pageSize).ConfigureAwait(false);
			if (latestMediaInTheaters.IsUnsuccessful)
			{
				return latestMediaInTheaters.Error;
			}
			
			return new List<FilteredMediaDto>
			{
				new()
				{
					PlaceName = placeName,
					PlaceId = null,
					Medias = latestMediaInTheaters.Value
				}
			};
		}
		
		var platform = await _platformRepository.FindAsync(placeId).ConfigureAwait(false);
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
				PlaceName = placeName,
				PlaceId = placeId,
				Medias = latestMediaResult.Value
			}
		};
	}
}