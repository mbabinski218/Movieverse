using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Common.Exceptions;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Domain.DomainEvents;

namespace Movieverse.Application.EventHandlers;

public sealed class UserBannedHandler : INotificationHandler<UserBanned>
{
	private readonly ILogger<PlatformToMediaAddedHandler> _logger;
	private readonly IMediaRepository _mediaRepository;

	public UserBannedHandler(ILogger<PlatformToMediaAddedHandler> logger, IMediaRepository mediaRepository)
	{
		_logger = logger;
		_mediaRepository = mediaRepository;
	}

	public async Task Handle(UserBanned notification, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("UserBannedHandler.Handle - User with id: {Id} has been banned", notification.UserId);
		
		var result = await _mediaRepository.BanReviewsByUserIdAsync(notification.UserId, cancellationToken);
		ResultException.ThrowIfUnsuccessful(result);
	}
}