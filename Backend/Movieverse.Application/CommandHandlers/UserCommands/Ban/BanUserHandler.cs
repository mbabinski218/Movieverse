using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Commands.User;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.DomainEvents;

namespace Movieverse.Application.CommandHandlers.UserCommands.Ban;

public sealed class BanUserHandler : IRequestHandler<BanUserCommand, Result>
{
	private readonly ILogger<BanUserHandler> _logger;
	private readonly IUserRepository _userRepository;
	private readonly IUnitOfWork _unitOfWork;

	public BanUserHandler(ILogger<BanUserHandler> logger, IUserRepository userRepository, IUnitOfWork unitOfWork)
	{
		_logger = logger;
		_userRepository = userRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result> Handle(BanUserCommand request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("BanUserCommandHandler.Handle - Ban user with id: {Id}", request.UserId);
		
		var user = await _userRepository.FindByIdAsync(request.UserId, cancellationToken);
		if (user.IsUnsuccessful)
		{
			return user.Error;
		}
		
		if (await _userRepository.IsSystemAdministratorAsync(user.Value, cancellationToken))
		{
			return Error.Invalid(UserResources.UserIsSystemAdministrator);
		}
		
		user.Value.Banned = true;
		await _userRepository.UpdateAsync(user.Value, cancellationToken);
		
		user.Value.AddDomainEvent(new UserBanned(request.UserId));
		
		if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
		{
			return Error.Invalid(UserResources.UserUpdateFailed);
		}

		return Result.Ok();
	}
}