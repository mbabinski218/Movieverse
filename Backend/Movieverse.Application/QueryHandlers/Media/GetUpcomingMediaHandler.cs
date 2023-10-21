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

	//TODO naprawić jak latest
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
		var movies = await _mediaRepository.GetUpcomingMoviesAsync(null, count, cancellationToken);
		if (movies.IsUnsuccessful)
		{
			return movies.Error;
		}

		return new List<FilteredMediaDto>
		{
			new()
			{
				PlatformId = Guid.Empty,
				PlatformName = "Theaters",
				Media = movies.Value
			}
		};
	}
	
	private async Task<Result<IEnumerable<FilteredMediaDto>>> UpcomingVodAsync(short count, CancellationToken cancellationToken)
	{
		var platforms = await _platformRepository.GetAllAsync(cancellationToken);
		if (platforms.IsUnsuccessful)
		{
			return platforms.Error;
		}
		
		var upcomingMedia = new List<FilteredMediaDto>();
		foreach (var platform in platforms.Value)
		{
			var media = await _mediaRepository.GetUpcomingMediaAsync(platform.Id.Value, count, cancellationToken);
			if (media.IsUnsuccessful)
			{
				continue;
			}
				
			upcomingMedia.Add(new FilteredMediaDto
			{
				PlatformId = platform.Id,
				PlatformName = platform.Name,
				Media = media.Value
			});
		}

		return upcomingMedia;
	}
}