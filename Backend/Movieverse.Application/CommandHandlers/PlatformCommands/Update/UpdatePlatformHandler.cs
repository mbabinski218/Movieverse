using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Commands.Platform;
using Movieverse.Contracts.DataTransferObjects.Platform;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.DomainEvents;

namespace Movieverse.Application.CommandHandlers.PlatformCommands.Update;

public sealed class UpdatePlatformHandler : IRequestHandler<UpdatePlatformCommand, Result<PlatformDto>>
{
	private readonly ILogger<UpdatePlatformHandler> _logger;
	private readonly IPlatformRepository _platformRepository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IOutputCacheStore _outputCacheStore;
	private readonly IMapper _mapper;

	public UpdatePlatformHandler(ILogger<UpdatePlatformHandler> logger, IPlatformRepository platformRepository, IUnitOfWork unitOfWork, 
		IOutputCacheStore outputCacheStore, IMapper mapper)
	{
		_logger = logger;
		_platformRepository = platformRepository;
		_unitOfWork = unitOfWork;
		_outputCacheStore = outputCacheStore;
		_mapper = mapper;
	}

	public async Task<Result<PlatformDto>> Handle(UpdatePlatformCommand request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Updating platform {id}...", request.Id);
		
		var findResult = await _platformRepository.FindAsync(request.Id, cancellationToken).ConfigureAwait(false);
		if (!findResult.IsSuccessful)
		{
			return findResult.Error;
		}
		
		var platform = findResult.Value;
		
		if (request.Name is not null) platform.Name = request.Name;
		if (request.Price is not null) platform.Price = request.Price.Value;
		if (request.Image is not null)
		{
			platform.AddDomainEvent(new ImageChanged(platform.LogoId, request.Image));
		}
		
		var updateResult = await _platformRepository.UpdateAsync(platform, cancellationToken).ConfigureAwait(false);
		if (!updateResult.IsSuccessful)
		{
			return updateResult.Error;
		}

		if (!await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false))
		{
			_logger.LogDebug("Platform {id} could not be updated.", request.Id);
			return Error.Invalid(PlatformResources.CannotUpdatePlatform);
		}
		
		await _outputCacheStore.EvictByTagAsync(request.Id.ToString(), cancellationToken).ConfigureAwait(false);
		
		_logger.LogDebug("Platform {id} updated successfully.", request.Id);
		return _mapper.Map<PlatformDto>(platform);
	}
}