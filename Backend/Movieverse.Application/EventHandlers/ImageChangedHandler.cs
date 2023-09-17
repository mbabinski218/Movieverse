using MediatR;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Common.Exceptions;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.DomainEvents;

namespace Movieverse.Application.EventHandlers;

public sealed class ImageChangedHandler : INotificationHandler<ImageChanged>
{
	private readonly ILogger<ImageChangedHandler> _logger;
	private readonly IContentRepository _contentRepository;
	private readonly IOutputCacheStore _outputCacheStore;
	private readonly IImageService _imageService;

	public ImageChangedHandler(ILogger<ImageChangedHandler> logger, IContentRepository contentRepository, IOutputCacheStore outputCacheStore, 
		IImageService imageService)
	{
		_logger = logger;
		_contentRepository = contentRepository;
		_outputCacheStore = outputCacheStore;
		_imageService = imageService;
	}

	public async Task Handle(ImageChanged notification, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Handling image changed event for image with id {id}.", notification.ImageId);
		
		var isInDatabaseResult = await _contentRepository.ExistsAsync(notification.ImageId, cancellationToken);
		ResultException.ThrowIfUnsuccessful(isInDatabaseResult);

		if (isInDatabaseResult.Value)
		{
			var deleteResult = await _imageService.DeleteImageAsync(notification.ImageId, cancellationToken);
			ResultException.ThrowIfUnsuccessful(deleteResult);

			var uploadResult = await _imageService.UploadImageAsync(notification.ImageId, notification.NewImage, cancellationToken);
			ResultException.ThrowIfUnsuccessful(uploadResult);

			await _outputCacheStore.EvictByTagAsync(notification.ImageId.ToString(), cancellationToken);
		}
		else
		{
			var content = Content.Create(notification.ImageId, notification.ImageId.ToString(), notification.NewImage.ContentType);
			var addResult = await _contentRepository.AddAsync(content, cancellationToken);
			ResultException.ThrowIfUnsuccessful(addResult);

			var uploadResult = await _imageService.UploadImageAsync(notification.ImageId, notification.NewImage, cancellationToken);
			ResultException.ThrowIfUnsuccessful(uploadResult);
		}
	}
}