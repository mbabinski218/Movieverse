using MassTransit;
using MediatR;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Common;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Commands.User;
using Movieverse.Contracts.Messages;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.CommandHandlers.UserCommands.ChangePassword;

public sealed class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, Result>
{
	private readonly ILogger<ChangePasswordHandler> _logger;
	private readonly IUserRepository _userRepository;
	private readonly IHttpService _httpService;
	private readonly IOutputCacheStore _outputCacheStore;
	private readonly IUnitOfWork _unitOfWork;

	public ChangePasswordHandler(ILogger<ChangePasswordHandler> logger, IUserRepository userRepository, IHttpService httpService, 
		IOutputCacheStore outputCacheStore, IUnitOfWork unitOfWork)
	{
		_logger = logger;
		_userRepository = userRepository;
		_httpService = httpService;
		_outputCacheStore = outputCacheStore;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
	{
		var id = _httpService.UserId;
		if (id is null)
		{
			return Error.Unauthorized(UserResources.UserDoesNotExist);
		}
		
		_logger.LogDebug("Changing password for user with id: {id}.", _httpService.UserId);
		
		var user = await _userRepository.FindByIdAsync(id.Value, cancellationToken);
		if (user.IsUnsuccessful)
		{
			_logger.LogDebug("Failed to change password for user with id: {id}.", _httpService.UserId);
			return user.Error;
		}
		
		var result = await _userRepository.ChangePasswordAsync(user.Value, request.CurrentPassword, request.NewPassword, cancellationToken);
		if (result.IsUnsuccessful)
		{
			_logger.LogDebug("Failed to change password for user with id: {id}.", _httpService.UserId);
			return result.Error;
		}
		
		_logger.LogDebug("Password changed for user with id: {id}.", _httpService.UserId);
		await _outputCacheStore.EvictByTagAsync(id.Value.ToString(), cancellationToken);
		return Result.Ok();
	}
}