using MediatR;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Common.Exceptions;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Domain.DomainEvents;

namespace Movieverse.Application.EventHandlers;

public sealed class PlatformToMediaAddedHandler : INotificationHandler<PlatformToMediaAdded>
{
	private readonly ILogger<PlatformToMediaAddedHandler> _logger;
	private readonly IPlatformRepository _platformRepository;
	private readonly IOutputCacheStore _outputCacheStore;

	public PlatformToMediaAddedHandler(ILogger<PlatformToMediaAddedHandler> logger, IPlatformRepository platformRepository, IOutputCacheStore outputCacheStore)
	{
		_logger = logger;
		_platformRepository = platformRepository;
		_outputCacheStore = outputCacheStore;
	}

	public async Task Handle(PlatformToMediaAdded notification, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Adding media with id {MediaId} to platform with id {PlatformId}", notification.MediaId.ToString(), notification.PlatformId.ToString());
		
		var platform = await _platformRepository.FindAsync(notification.PlatformId, cancellationToken);
		ResultException.ThrowIfUnsuccessful(platform);
		
		platform.Value.AddMedia(notification.MediaId);
		await _outputCacheStore.EvictByTagAsync(notification.PlatformId.ToString(), cancellationToken);
	}
}