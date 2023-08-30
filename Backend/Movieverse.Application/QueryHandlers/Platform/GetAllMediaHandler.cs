using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Contracts.Queries.Platform;
using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Common;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.QueryHandlers.Platform;

public sealed class GetAllMediaHandler : IRequestHandler<GetAllMediaQuery, Result<IPaginatedList<MediaInfoDto>>>
{
	private readonly ILogger<GetAllMediaHandler> _logger;
	private readonly IPlatformRepository _platformRepository;
	private readonly IMediaRepository _mediaRepository;

	public GetAllMediaHandler(ILogger<GetAllMediaHandler> logger, IPlatformRepository platformRepository, IMediaRepository mediaRepository)
	{
		_logger = logger;
		_platformRepository = platformRepository;
		_mediaRepository = mediaRepository;
	}

	public async Task<Result<IPaginatedList<MediaInfoDto>>> Handle(GetAllMediaQuery request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Getting all media for platform with id {Id}", request.Id);
		
		var mediaIds = await _platformRepository.GetAllMediaIdsAsync(request.Id, cancellationToken).ConfigureAwait(false);
		if (mediaIds.IsUnsuccessful)
		{
			return mediaIds.Error;
		}
		
		return request.Type switch
		{
			nameof(Movie) => await _mediaRepository.FindMoviesByIdsAsync(mediaIds.Value, request.PageNumber, request.PageSize, cancellationToken).ConfigureAwait(false),
			nameof(Series) => await _mediaRepository.FindSeriesByIdsAsync(mediaIds.Value, request.PageNumber, request.PageSize, cancellationToken).ConfigureAwait(false),
			_ => Error.Invalid("Invalid media type")
		};
	}
}