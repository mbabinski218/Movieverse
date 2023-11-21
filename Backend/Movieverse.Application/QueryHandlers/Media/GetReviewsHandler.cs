using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Contracts.Queries.Media;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.QueryHandlers.Media;

public sealed class GetReviewsHandler : IRequestHandler<GetReviewQuery, Result<IEnumerable<ReviewDto>>>
{
	private readonly ILogger<GetReviewsHandler> _logger;
	private readonly IMediaReadOnlyRepository _mediaRepository;

	public GetReviewsHandler(ILogger<GetReviewsHandler> logger, IMediaReadOnlyRepository mediaRepository)
	{
		_logger = logger;
		_mediaRepository = mediaRepository;
	}

	public async Task<Result<IEnumerable<ReviewDto>>> Handle(GetReviewQuery request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("GetReviewsHandler.Handle - Retrieving reviews for media with id: {Id}", request.Id);
		
		return await _mediaRepository.GetReviewsAsync(request.Id, cancellationToken);
	}
}