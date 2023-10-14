using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Contracts.Queries.Platform;
using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Common;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.QueryHandlers.Platform;

public sealed class GetAllMediaHandler : IRequestHandler<GetAllMediaQuery, Result<IPaginatedList<MediaInfoDto>>>
{
	private readonly ILogger<GetAllMediaHandler> _logger;
	private readonly IPlatformReadOnlyRepository _platformRepository;
	private readonly IMediaReadOnlyRepository _mediaRepository;

	public GetAllMediaHandler(ILogger<GetAllMediaHandler> logger, IPlatformReadOnlyRepository platformRepository, 
		IMediaReadOnlyRepository mediaRepository)
	{
		_logger = logger;
		_platformRepository = platformRepository;
		_mediaRepository = mediaRepository;
	}

	public async Task<Result<IPaginatedList<MediaInfoDto>>> Handle(GetAllMediaQuery request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Getting all media for platform with id {Id}", request.Id);
		
		var mediaIds = await _platformRepository.GetAllMediaIdsAsync(request.Id, cancellationToken);
		if (mediaIds.IsUnsuccessful)
		{
			return mediaIds.Error;
		}
		
		return request.Type switch
		{
			nameof(Movie) => await _mediaRepository
				.FindMoviesByIdsAsync(mediaIds.Value.ToList(), request.PageNumber, request.PageSize, cancellationToken)
				.ConfigureAwait(false),
			nameof(Series) => await _mediaRepository
				.FindSeriesByIdsAsync(mediaIds.Value.ToList(), request.PageNumber, request.PageSize, cancellationToken)
				.ConfigureAwait(false),
			_ => Error.Invalid("Invalid media type")
		};
	}
}