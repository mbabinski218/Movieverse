using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Common.Exceptions;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Domain.DomainEvents;

namespace Movieverse.Application.EventHandlers;

public sealed class NewRatingAddedHandler : INotificationHandler<NewRatingAdded>
{
	private readonly ILogger<NewRatingAddedHandler> _logger;
	private readonly IMediaRepository _mediaRepository;

	public NewRatingAddedHandler(ILogger<NewRatingAddedHandler> logger, IMediaRepository mediaRepository)
	{
		_logger = logger;
		_mediaRepository = mediaRepository;
	}
	
	public async Task Handle(NewRatingAdded notification, CancellationToken cancellationToken)
	{
		_logger.LogDebug("NewRatingAddedHandler.Handle");
		
		var media = await _mediaRepository.FindAsync(notification.MediaId, cancellationToken);
		ResultException.ThrowIfUnsuccessful(media);

		media.Value.BasicStatistics.Votes++;
	}
}