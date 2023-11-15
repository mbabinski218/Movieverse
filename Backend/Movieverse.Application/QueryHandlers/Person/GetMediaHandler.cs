using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Contracts.Queries.Person;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.QueryHandlers.Person;

public sealed class GetMediaHandler : IRequestHandler<GetMediaQuery, Result<IEnumerable<MediaSectionDto>>>
{
	private readonly ILogger<GetMediaHandler> _logger;
	private readonly IPersonReadOnlyRepository _personRepository;
	private readonly IMediaReadOnlyRepository _mediaRepository;

	public GetMediaHandler(ILogger<GetMediaHandler> logger, IPersonReadOnlyRepository personRepository, IMediaReadOnlyRepository mediaRepository)
	{
		_logger = logger;
		_personRepository = personRepository;
		_mediaRepository = mediaRepository;
	}

	public async Task<Result<IEnumerable<MediaSectionDto>>> Handle(GetMediaQuery request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Retrieving media for person with id {Id}", request.Id);
		
		var mediaIds = await _personRepository.GetMediaIdsAsync(request.Id, cancellationToken);
		if (mediaIds.IsUnsuccessful)
		{
			return mediaIds.Error;
		}

		return await _mediaRepository.GetMediaSectionAsync(mediaIds.Value, cancellationToken);
	}
}