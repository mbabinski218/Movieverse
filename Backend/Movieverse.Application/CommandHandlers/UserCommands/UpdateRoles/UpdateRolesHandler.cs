using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Commands.User;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.Common.Types;

namespace Movieverse.Application.CommandHandlers.UserCommands.UpdateRoles;

public sealed class UpdateRolesHandler : IRequestHandler<UpdateRolesCommand, Result>
{
	private readonly ILogger<UpdateRolesHandler> _logger;
	private readonly IUserRepository _userRepository;

	public UpdateRolesHandler(ILogger<UpdateRolesHandler> logger, IUserRepository userRepository)
	{
		_logger = logger;
		_userRepository = userRepository;
	}

	public async Task<Result> Handle(UpdateRolesCommand request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Updating roles for user with email {Email}", request.Email);
		
		var user = await _userRepository.FindByEmailAsync(request.Email, cancellationToken);
		if (user.IsUnsuccessful)
		{
			return user.Error;
		}

		if (request.Roles.Contains(UserRole.SystemAdministrator.ToStringFast()))
		{
			return Error.Invalid(UserResources.SystemAdministratorRoleCannotBeAdded);
		}
		
		return await _userRepository.UpdateRolesAsync(user.Value, request.Roles.ToList(), cancellationToken);
	}
}