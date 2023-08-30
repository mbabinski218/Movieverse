using MediatR;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Contracts.Commands.Platform;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.DomainEvents;

namespace Movieverse.Application.CommandHandlers.PlatformCommands.AddMedia;

public sealed class AddMediaToPlatformHandler : IRequestHandler<AddMediaToPlatformCommand, Result>
{
	private readonly ILogger<AddMediaToPlatformHandler> _logger;
	private readonly IPlatformRepository _platformRepository;
	private readonly IMediaRepository _mediaRepository;
	private readonly IOutputCacheStore _outputCacheStore;
	private readonly IUnitOfWork _unitOfWork;

	public AddMediaToPlatformHandler(ILogger<AddMediaToPlatformHandler> logger, IPlatformRepository platformRepository, 
		IMediaRepository mediaRepository, IOutputCacheStore outputCacheStore, IUnitOfWork unitOfWork)
	{
		_logger = logger;
		_platformRepository = platformRepository;
		_mediaRepository = mediaRepository;
		_outputCacheStore = outputCacheStore;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result> Handle(AddMediaToPlatformCommand request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Adding media with id {MediaId} to platform with id {PlatformId}", request.MediaId, request.Id);
		
		if (!await _mediaRepository.ExistsAsync(request.MediaId, cancellationToken).ConfigureAwait(false))
		{
			return Error.NotFound("Media does not exist");
		}
		
		var platform = await _platformRepository.FindByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
		if (platform.IsUnsuccessful)
		{
			return platform.Error;
		}
		
		platform.Value.MediaIds.Add(request.MediaId);
		platform.Value.AddDomainEvent(new MediaToPlatformAdded(request.Id, request.MediaId));
		
		var updateResult = await _platformRepository.UpdateAsync(platform.Value, cancellationToken).ConfigureAwait(false);
		if (updateResult.IsUnsuccessful)
		{
			return updateResult.Error;
		}
		
		if (!await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false))
		{
			_logger.LogError("Could not add media with id {MediaId} to platform with id {PlatformId}", request.MediaId, request.Id);
			return Error.Invalid("Could not add media to platform");
		}
		
		await _outputCacheStore.EvictByTagAsync(request.Id.ToString(), cancellationToken).ConfigureAwait(false);
		return Result.Ok();
	}
}