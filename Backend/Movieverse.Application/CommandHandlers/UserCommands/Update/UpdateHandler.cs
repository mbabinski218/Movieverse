using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Commands.User;
using Movieverse.Contracts.DataTransferObjects.User;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.DomainEvents;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Application.CommandHandlers.UserCommands.Update;

public sealed class UpdateHandler : IRequestHandler<UpdateCommand, Result<UserDto>>
{
	private readonly ILogger<UpdateHandler> _logger;
	private readonly IUserRepository _userRepository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly IOutputCacheStore _outputCacheStore;
	private readonly IHttpService _httpService;

	public UpdateHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper, IOutputCacheStore outputCacheStore, 
		ILogger<UpdateHandler> logger, IHttpService httpService)
	{
		_userRepository = userRepository;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_outputCacheStore = outputCacheStore;
		_logger = logger;
		_httpService = httpService;
	}

	public async Task<Result<UserDto>> Handle(UpdateCommand request, CancellationToken cancellationToken)
	{
		var id = _httpService.UserId;
		if (id is null)
		{
			return Error.Unauthorized(UserResources.UserDoesNotExist);
		}
		
		_logger.LogDebug("Updating user with id: {id}...", id);
		
		var findResult = await _userRepository.FindByIdAsync(id.Value, cancellationToken);
		if (!findResult.IsSuccessful)
		{
			return findResult.Error;
		}
		
		var user = findResult.Value;

		if (request.UserName is not null)
		{
			var result = await _userRepository.ChangeUsernameAsync(user, request.UserName, cancellationToken);
			if (result.IsUnsuccessful)
			{
				return result.Error;
			}
		}
		if (request.Email is not null)
		{
			var result = await _userRepository.ChangeEmailAsync(user, request.Email, cancellationToken);
			if (result.IsUnsuccessful)
			{
				return result.Error;
			}
		}
		if (request.Information is not null)
		{
			if (request.Information.FirstName is not null) user.Information.FirstName = request.Information.FirstName;
			if (request.Information.LastName is not null) user.Information.LastName = request.Information.LastName;
			if (request.Information.Age is not null) user.Information.Age = request.Information.Age.Value;
		}
		
		if (request.Avatar is not null)
		{
			if (user.AvatarId is null)
			{
				var avatarId = ContentId.Create();
				user.AddDomainEvent(new ImageChanged(avatarId, request.Avatar));
				user.AvatarId = avatarId;
			}
			else
			{
				user.AddDomainEvent(new ImageChanged(user.AvatarId, request.Avatar));
			}
		}
		
		var updateResult = await _userRepository.UpdateAsync(user, cancellationToken);
		if (!updateResult.IsSuccessful)
		{
			return updateResult.Error;
		}

		if (!await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false))
		{
			_logger.LogDebug("User {id} could not be updated.", id);
			return Error.Invalid(UserResources.UserUpdateFailed);
		}
		
		await _outputCacheStore.EvictByTagAsync(id.Value.ToString(), cancellationToken);
		
		_logger.LogDebug("User {id} updated successfully.", id);
        return _mapper.Map<UserDto>(user);
	}
}