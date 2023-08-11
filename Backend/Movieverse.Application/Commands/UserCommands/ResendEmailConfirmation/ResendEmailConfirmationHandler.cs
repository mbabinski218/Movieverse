using System.Web;
using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Common.Result;
using Movieverse.Application.Interfaces;

namespace Movieverse.Application.Commands.UserCommands.ResendEmailConfirmation;

public sealed class ResendEmailConfirmationHandler : IRequestHandler<ResendEmailConfirmationCommand, Result>
{
	private readonly ILogger<ResendEmailConfirmationCommand> _logger;
	private readonly IUserRepository _userRepository;
	private readonly IHttpService _httpService;
	private readonly IEmailServiceProvider _emailServiceProvider;

	public ResendEmailConfirmationHandler(ILogger<ResendEmailConfirmationCommand> logger, IUserRepository userRepository, 
		IHttpService httpService, IEmailServiceProvider emailServiceProvider)
	{
		_logger = logger;
		_userRepository = userRepository;
		_httpService = httpService;
		_emailServiceProvider = emailServiceProvider;
	}

	public async Task<Result> Handle(ResendEmailConfirmationCommand request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Resending email confirmation link to: {email}", request.Email);
		
		var user = await _userRepository.FindByEmailAsync(request.Email);

		if (!user.IsSuccessful) return user.Error;

		var token = await _userRepository.GenerateEmailConfirmationTokenAsync(user.Value);

		if (!token.IsSuccessful) return token.Error;
		
		var url = _httpService.Uri?.GetLeftPart(UriPartial.Authority);
		var id = user.Value.Id.ToString();
		var encodedToken = HttpUtility.UrlEncode(token.Value);
		var link = $"{url}/api/user/confirm-email?Id={id}&Token={encodedToken}";
		
		var result = await _emailServiceProvider.SendEmailConfirmationAsync(request.Email, link, cancellationToken);
		
		return result.IsSuccessful ? Result.Ok() : result.Error;
	}
}