using MediatR;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Common.Exceptions;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Domain.DomainEvents;

namespace Movieverse.Application.EventHandlers;

public sealed class MediaToPlatformAddedHandler : INotificationHandler<MediaToPlatformAdded>
{
	private readonly ILogger<MediaToPlatformAddedHandler> _logger;
	private readonly IMediaRepository _mediaRepository;
	private readonly IOutputCacheStore _outputCacheStore;

	public MediaToPlatformAddedHandler(ILogger<MediaToPlatformAddedHandler> logger, IMediaRepository mediaRepository, IOutputCacheStore outputCacheStore)
	{
		_logger = logger;
		_mediaRepository = mediaRepository;
		_outputCacheStore = outputCacheStore;
	}

	public async Task Handle(MediaToPlatformAdded notification, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Adding media with id {MediaId} to platform with id {PlatformId}", notification.MediaId, notification.PlatformId);
		
		var media = await _mediaRepository.FindAsync(notification.MediaId, cancellationToken);
		ResultException.ThrowIfUnsuccessful(media);
		
		media.Value.AddPlatform(notification.PlatformId);
		await _outputCacheStore.EvictByTagAsync(notification.MediaId.ToString(), cancellationToken);
	}
}