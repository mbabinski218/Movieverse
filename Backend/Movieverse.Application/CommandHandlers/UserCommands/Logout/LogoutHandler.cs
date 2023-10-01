using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Commands.User;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.CommandHandlers.UserCommands.Logout;

public sealed class LogoutHandler : IRequestHandler<LogoutCommand, Result>
{
	private readonly ILogger<LogoutHandler> _logger;
	private readonly IUserRepository _userRepository;
	private readonly IHttpService _httpService;

	public LogoutHandler(ILogger<LogoutHandler> logger, IUserRepository userRepository, IHttpService httpService)
	{
		_logger = logger;
		_httpService = httpService;
		_userRepository = userRepository;
	}

	public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
	{
		var userId = _httpService.UserId;
		if (userId is null)
		{
			_logger.LogDebug("LogoutHandler.Handle - User is not logged in");
			return Error.Unauthorized(UserResources.YouAreNotLoggedIn);
		}
		
		_logger.LogDebug("User with id {UserId} is logging out", userId.ToString());
		
		var userResult = await _userRepository.FindByIdAsync(userId.Value, cancellationToken).ConfigureAwait(false);
		return userResult.IsSuccessful 
			? await _userRepository.LogoutAsync(userResult.Value, cancellationToken).ConfigureAwait(false)
			: userResult.Error;
	}
}