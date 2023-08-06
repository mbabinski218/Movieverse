using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Common.Result;
using Movieverse.Application.Interfaces;
using Movieverse.Domain.AggregateRoots;

namespace Movieverse.Application.Commands.UserCommands.Register;

public sealed class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Result>
{
	private readonly ILogger<RegisterUserHandler> _logger;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IUserRepository _userRepository;

	public RegisterUserHandler(ILogger<RegisterUserHandler> logger, IUnitOfWork unitOfWork, IUserRepository userRepository)
	{
		_logger = logger;
		_unitOfWork = unitOfWork;
		_userRepository = userRepository;
	}

	public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
	{
		var user = User.Create(request.Email, request.UserName, request.FirstName, request.LastName, request.Age);
		
		var result = await _userRepository.RegisterAsync(user, request.Password);

		if (!result.IsSuccessful) return result;
		
		await _unitOfWork.SaveChangesAsync(cancellationToken);
		_logger.LogDebug("User {email} registered successfully.", request.Email);

		return result;
	}
}