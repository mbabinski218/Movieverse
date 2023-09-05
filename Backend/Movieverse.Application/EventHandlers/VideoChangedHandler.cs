using MediatR;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Common.Exceptions;
using Movieverse.Application.Interfaces;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.DomainEvents;

namespace Movieverse.Application.EventHandlers;

public sealed class VideoChangedHandler : INotificationHandler<VideoChanged>
{
	private readonly ILogger<VideoChangedHandler> _logger;
	private readonly IContentRepository _contentRepository;
	private readonly IOutputCacheStore _outputCacheStore;

	public VideoChangedHandler(ILogger<VideoChangedHandler> logger, IContentRepository contentRepository, IOutputCacheStore outputCacheStore)
	{
		_logger = logger;
		_contentRepository = contentRepository;
		_outputCacheStore = outputCacheStore;
	}

	public async Task Handle(VideoChanged notification, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Handling video changed event for video with id {id}.", notification.VideoId);
		
		var isInDatabaseResult = await _contentRepository.ExistsAsync(notification.VideoId, cancellationToken);
		ResultException.ThrowIfUnsuccessful(isInDatabaseResult);

		if (isInDatabaseResult.Value)
		{
			var content = await _contentRepository.FindAsync(notification.VideoId, cancellationToken);
			ResultException.ThrowIfUnsuccessful(content);
			
			content.Value.Path = notification.NewVideo;
			var updateResult = await _contentRepository.UpdateAsync(content.Value, cancellationToken);
			ResultException.ThrowIfUnsuccessful(updateResult);
		}
		else
		{
			var content = Content.Create(notification.VideoId, notification.NewVideo, "video");
			var addResult = await _contentRepository.AddAsync(content, cancellationToken);
			ResultException.ThrowIfUnsuccessful(addResult);
		}
		
		await _outputCacheStore.EvictByTagAsync(notification.VideoId.ToString(), cancellationToken);
	}
}