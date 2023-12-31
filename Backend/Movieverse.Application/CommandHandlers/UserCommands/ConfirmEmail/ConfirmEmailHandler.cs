﻿using System.Web;
using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Contracts.Commands.User;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.CommandHandlers.UserCommands.ConfirmEmail;

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
		
		var user = await _userRepository.FindByIdAsync(request.Id, cancellationToken);

		if (!user.IsSuccessful) return user.Error;
		
		var decodedToken = HttpUtility.UrlDecode(request.Token);
		
		var result = await _userRepository.ConfirmEmailAsync(user.Value, decodedToken, cancellationToken);
		
		return result.IsSuccessful ? Result.Ok() : result.Error;
	}
}