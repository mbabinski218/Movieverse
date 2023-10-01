using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Contracts.Queries.Media;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.QueryHandlers.Media;

public sealed class GetMediaHandler : IRequestHandler<GetMediaQuery, Result<MediaDto>>
{
	private readonly ILogger<GetMediaHandler> _logger;
	private readonly IMediaReadOnlyRepository _mediaRepository;

	public GetMediaHandler(ILogger<GetMediaHandler> logger, IMediaReadOnlyRepository mediaRepository)
	{
		_logger = logger;
		_mediaRepository = mediaRepository;
	}

	public async Task<Result<MediaDto>> Handle(GetMediaQuery request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Getting media with id {Id}", request.Id);
		
		var media = await _mediaRepository.FindAsync(request.Id, cancellationToken).ConfigureAwait(false);
		return media.IsSuccessful ? media.Value : media.Error;
	}
}