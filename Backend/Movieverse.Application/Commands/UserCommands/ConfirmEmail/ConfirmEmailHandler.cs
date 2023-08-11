using System.Web;
using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Common.Result;
using Movieverse.Application.Interfaces;

namespace Movieverse.Application.Commands.UserCommands.ConfirmEmail;

public sealed class ConfirmEmailHandler : IRequestHandler<ConfirmEmailCommand, Result>
{
	private readonly ILogger<ConfirmEmailHandler> _logger;
	private readonly IUserRepository _userRepository;

	public ConfirmEmailHandler(ILogger<ConfirmEmailHandler> logger, IUserRepository userRepository)
	{
		_logger = logger;
		_userRepository = userRepository;
	}

	public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Confirming email for user with id: {Id}", request.Id);
		
		var user = await _userRepository.FindByIdAsync(request.Id);

		if (!user.IsSuccessful)
		{
			return user.Error;
		}
		
		var decodedToken = HttpUtility.UrlDecode(request.Token);
		
		var result = await _userRepository.ConfirmEmailAsync(user.Value, decodedToken);
		
		return result.IsSuccessful ? Result.Ok() : result.Error;
	}
}