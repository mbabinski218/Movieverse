using MediatR;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Commands.Platform;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.DomainEvents;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Application.CommandHandlers.PlatformCommands.Add;

public sealed class AddPlatformHandler : IRequestHandler<AddPlatformCommand, Result>
{
	private readonly ILogger<AddPlatformHandler> _logger;
	private readonly IPlatformRepository _platformRepository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IOutputCacheStore _outputCacheStore;

	public AddPlatformHandler(ILogger<AddPlatformHandler> logger, IPlatformRepository platformRepository, IUnitOfWork unitOfWork, 
		IOutputCacheStore outputCacheStore)
	{
		_logger = logger;
		_platformRepository = platformRepository;
		_unitOfWork = unitOfWork;
		_outputCacheStore = outputCacheStore;
	}

	public async Task<Result> Handle(AddPlatformCommand request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Adding platform {name}...", request.Name);
		
		var imageId = ContentId.Create();
        var platform = Platform.Create(request.Name, imageId, request.Price);
        platform.AddDomainEvent(new ImageChanged(imageId, request.Image));

        var addResult = await _platformRepository.AddAsync(platform, cancellationToken);
        if (addResult.IsUnsuccessful)
		{
	        _logger.LogDebug("Platform {name} could not be added.", request.Name);
	        return addResult.Error;
		}

        if (!await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false))
        {
	        _logger.LogDebug("Platform {name} could not be added.", request.Name);
	        return Error.Invalid(PlatformResources.CannotCreatePlatform);
        }
        
		await _outputCacheStore.EvictByTagAsync(platform.Id.ToString(), cancellationToken);
		
		_logger.LogDebug("Platform {name} added successfully.", request.Name);
		return Result.Ok();
	}
}