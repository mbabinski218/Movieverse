using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Contracts.Queries.Media;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.QueryHandlers.Media;

public sealed class GetUpcomingMediaHandler : IRequestHandler<GetUpcomingMediaQuery, Result<IEnumerable<FilteredMediaDto>>>
{
	private readonly ILogger<GetUpcomingMediaHandler> _logger;
	private readonly IMediaReadOnlyRepository _mediaRepository;
	private readonly IPlatformReadOnlyRepository _platformRepository;

	public GetUpcomingMediaHandler(ILogger<GetUpcomingMediaHandler> logger, IMediaReadOnlyRepository mediaRepository, 
		IPlatformReadOnlyRepository platformRepository)
	{
		_logger = logger;
		_mediaRepository = mediaRepository;
		_platformRepository = platformRepository;
	}

	public async Task<Result<IEnumerable<FilteredMediaDto>>> Handle(GetUpcomingMediaQuery request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Getting upcoming media...");

		return request.Place switch
		{
			"Theaters" => await UpcomingTheatersAsync(request.Count, cancellationToken).ConfigureAwait(false),
			"Vod" => await UpcomingVodAsync(request.Count, cancellationToken).ConfigureAwait(false),
			_ => Error.NotFound(MediaResources.PlaceNotFound)
		};
	}

	private async Task<Result<IEnumerable<FilteredMediaDto>>> UpcomingTheatersAsync(short count, CancellationToken cancellationToken)
	{
		var movies = await _mediaRepository.GetUpcomingMoviesAsync(null, count, cancellationToken).ConfigureAwait(false);
		if (movies.IsUnsuccessful)
		{
			return movies.Error;
		}

		return new List<FilteredMediaDto>
		{
			new()
			{
				PlaceName = "Theaters",
				PlaceId = null,
				Medias = movies.Value
			}
		};
	}
	
	private async Task<Result<IEnumerable<FilteredMediaDto>>> UpcomingVodAsync(short count, CancellationToken cancellationToken)
	{
		var platforms = await _platformRepository.GetAllAsync(cancellationToken).ConfigureAwait(false);
		if (platforms.IsUnsuccessful)
		{
			return platforms.Error;
		}
		
		var upcomingMedia = new List<FilteredMediaDto>();
		foreach (var platform in platforms.Value)
		{
			var medias = await _mediaRepository.GetUpcomingMediaAsync(platform.Id.Value, count, cancellationToken).ConfigureAwait(false);
			if (medias.IsUnsuccessful)
			{
				continue;
			}
				
			upcomingMedia.Add(new FilteredMediaDto
			{
				PlaceName = platform.Name,
				PlaceId = platform.Id,
				Medias = medias.Value
			});
		}

		return upcomingMedia;
	}
}