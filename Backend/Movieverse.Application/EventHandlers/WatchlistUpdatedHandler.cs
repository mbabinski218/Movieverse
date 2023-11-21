using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Common.Exceptions;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Domain.DomainEvents;

namespace Movieverse.Application.EventHandlers;

public sealed class WatchlistUpdatedHandler : INotificationHandler<WatchlistUpdated>
{
	private readonly ILogger<WatchlistUpdatedHandler> _logger;
	private readonly IMediaRepository _mediaRepository;

	public WatchlistUpdatedHandler(ILogger<WatchlistUpdatedHandler> logger, IMediaRepository mediaRepository)
	{
		_logger = logger;
		_mediaRepository = mediaRepository;
	}

	public async Task Handle(WatchlistUpdated notification, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("WatchlistUpdatedHandler.Handle");
		
		var media = await _mediaRepository.FindAsync(notification.MediaId, cancellationToken);
		ResultException.ThrowIfUnsuccessful(media);
		
		media.Value.BasicStatistics.OnWatchlistCount += notification.Status ? 1 : -1;
	}
}