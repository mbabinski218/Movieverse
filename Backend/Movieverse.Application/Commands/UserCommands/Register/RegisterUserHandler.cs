using MediatR;
using Movieverse.Application.Common.Result;
using Movieverse.Application.Interfaces;

namespace Movieverse.Application.Commands.UserCommands.Register;

public sealed class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Result>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IUserRepository _userRepository;

	public RegisterUserHandler(IUnitOfWork unitOfWork, IUserRepository userRepository)
	{
		_unitOfWork = unitOfWork;
		_userRepository = userRepository;
	}

	public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
	{
		return await _userRepository.RegisterAsync();
	}
}