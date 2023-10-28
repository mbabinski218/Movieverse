using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Common.Exceptions;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Domain.DomainEvents;

namespace Movieverse.Application.EventHandlers;

public sealed class PersonalityCreatedHandler : INotificationHandler<PersonalityCreated>
{
	private readonly ILogger<PersonalityCreatedHandler> _logger;
	private readonly IUserRepository _userRepository;

	public PersonalityCreatedHandler(ILogger<PersonalityCreatedHandler> logger, IUserRepository userRepository)
	{
		_logger = logger;
		_userRepository = userRepository;
	}

	public async Task Handle(PersonalityCreated notification, CancellationToken cancellationToken)
	{
		_logger.LogDebug("PersonalityCreatedHandler.Handle called");
		
		var addResult = await _userRepository.AddPersonalityAsync(notification.UserId, notification.PersonId, cancellationToken);
		ResultException.ThrowIfUnsuccessful(addResult);
		
		_logger.LogDebug("Personality created for user with id {UserId}", notification.UserId);
	}
}