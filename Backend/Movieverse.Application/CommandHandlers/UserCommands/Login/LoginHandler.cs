﻿using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Commands.User;
using Movieverse.Contracts.DataTransferObjects.User;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.Common.Types;

namespace Movieverse.Application.CommandHandlers.UserCommands.Login;

public sealed class LoginHandler : IRequestHandler<LoginCommand, Result<TokensDto>>
{
	private readonly ILogger<LoginHandler> _logger;
	private readonly IUserRepository _userRepository;

	public LoginHandler(ILogger<LoginHandler> logger, IUserRepository userRepository)
	{
		_logger = logger;
		_userRepository = userRepository;
	}

	public async Task<Result<TokensDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Login user with email: {email}", request.Email);

		if (!GrantTypeExtensions.TryParse(request.GrantType, out var grantType))
		{
			return Error.Invalid("Invalid grant type");
		}
		
		switch (grantType)
		{
			case GrantType.Password:
				var user = await _userRepository.FindByEmailAsync(request.Email!, cancellationToken).ConfigureAwait(false);
				if (user.IsUnsuccessful)
				{
					return user.Error;
				}
				return await _userRepository.LoginAsync(user.Value, request.Password!, cancellationToken).ConfigureAwait(false);
			
			case GrantType.RefreshToken:
				user = await _userRepository.FindByRefreshTokenAsync(request.RefreshToken!, cancellationToken).ConfigureAwait(false);
				if (user.IsUnsuccessful)
				{
					return user.Error;
				}
				return await _userRepository.LoginWithRefreshTokenAsync(user.Value, request.RefreshToken!, cancellationToken).ConfigureAwait(false);
			
			case GrantType.Google:
				return await _userRepository.LoginWithGoogleAsync(request.IdToken!, cancellationToken).ConfigureAwait(false);
			
			case GrantType.Facebook:
				return await _userRepository.LoginWithFacebookAsync(request.IdToken!, cancellationToken).ConfigureAwait(false);
			
			default:
				return Error.Invalid(UserResources.InvalidGrantType);
		};
	}
}