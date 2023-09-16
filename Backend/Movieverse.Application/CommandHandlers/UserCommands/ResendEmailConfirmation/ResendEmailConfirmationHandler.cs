using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Common;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Contracts.Commands.User;
using Movieverse.Contracts.Messages;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.CommandHandlers.UserCommands.ResendEmailConfirmation;

public sealed class ResendEmailConfirmationHandler : IRequestHandler<ResendEmailConfirmationCommand, Result>
{
	private readonly ILogger<ResendEmailConfirmationCommand> _logger;
	private readonly IUserRepository _userRepository;
	private readonly IHttpService _httpService;
	private readonly IBus _bus;

	public ResendEmailConfirmationHandler(ILogger<ResendEmailConfirmationCommand> logger, IUserRepository userRepository, 
		IHttpService httpService, IBus bus)
	{
		_logger = logger;
		_userRepository = userRepository;
		_httpService = httpService;
		_bus = bus;
	}

	public async Task<Result> Handle(ResendEmailConfirmationCommand request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Resending email confirmation link to: {email}", request.Email);
		
		var user = await _userRepository.FindByEmailAsync(request.Email, cancellationToken);

		if (!user.IsSuccessful) return user.Error;

		var token = await _userRepository.GenerateEmailConfirmationTokenAsync(user.Value, cancellationToken);

		if (!token.IsSuccessful) return token.Error;
		
		var url = _httpService.Uri?.GetLeftPart(UriPartial.Authority);
		var emailConfirmationLink = EmailHelper.CreateConfirmationLink(url, user.Value.Id, token.Value);
        await _bus.Publish(new UserRegisteredMessage(request.Email, emailConfirmationLink), cancellationToken);
		
		return Result.Ok();
	}
}