﻿using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Common;
using Movieverse.Application.Interfaces;
using Movieverse.Contracts.Commands.User;
using Movieverse.Contracts.Messages;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.CommandHandlers.UserCommands.Register;

public sealed class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Result>
{
	private readonly ILogger<RegisterUserHandler> _logger;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IUserRepository _userRepository;
	private readonly IBus _bus;
	private readonly IHttpService _httpService;

	public RegisterUserHandler(ILogger<RegisterUserHandler> logger, IUnitOfWork unitOfWork, IUserRepository userRepository, IBus bus, 
		IHttpService httpService)
	{
		_logger = logger;
		_unitOfWork = unitOfWork;
		_userRepository = userRepository;
		_bus = bus;
		_httpService = httpService;
	}

	public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
	{
		var user = User.Create(request.Email, request.UserName, request.FirstName, request.LastName, request.Age);
		
		var token = await _userRepository.RegisterAsync(user, request.Password);

		if (!token.IsSuccessful) return token.Error;
		
		var url = _httpService.Uri?.GetLeftPart(UriPartial.Authority);
		var emailConfirmationLink = EmailHelper.CreateConfirmationLink(url, user.Id, token.Value);
		await _bus.Publish(new UserRegisteredMessage(user.Email!, emailConfirmationLink), cancellationToken);
			
		_logger.LogDebug("User {email} registered successfully.", request.Email);
		return Result.Ok();
	}
}