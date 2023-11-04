using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Contracts.DataTransferObjects.Content;
using Movieverse.Contracts.Queries.Content;
using Movieverse.Contracts.Queries.Media;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.QueryHandlers.Media;

public sealed class GetContentPathHandler : IRequestHandler<GetContentPath, Result<IEnumerable<ContentInfoDto>>>
{
	private readonly ILogger<GetContentPathHandler> _logger;
	private readonly IMediaReadOnlyRepository _mediaRepository;
	private readonly IContentReadOnlyRepository _contentRepository;

	public GetContentPathHandler(IContentReadOnlyRepository contentRepository, ILogger<GetContentPathHandler> logger, IMediaReadOnlyRepository mediaRepository)
	{
		_logger = logger;
		_mediaRepository = mediaRepository;
		_contentRepository = contentRepository;
	}

	public async Task<Result<IEnumerable<ContentInfoDto>>> Handle(GetContentPath request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Getting content for media id: ", request.Id);

		var contentIds = await _mediaRepository.GetContentAsync(request.Id, cancellationToken);
		if (contentIds.IsUnsuccessful)
		{
			return contentIds.Error;
		}
		
		var result = await _contentRepository.GetPathsAsync(contentIds.Value, cancellationToken);
		return result;
	}
}