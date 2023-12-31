﻿using MediatR;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Common.Exceptions;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Domain.DomainEvents;

namespace Movieverse.Application.EventHandlers;

public sealed class PersonFromMediaRemovedHandler : INotificationHandler<PersonFromMediaRemoved>
{
	private readonly ILogger<PersonFromMediaRemovedHandler> _logger;
	private readonly IPersonRepository _personRepository;
	private readonly IOutputCacheStore _outputCacheStore;

	public PersonFromMediaRemovedHandler(ILogger<PersonFromMediaRemovedHandler> logger, IPersonRepository personRepository, 
		IOutputCacheStore outputCacheStore)
	{
		_logger = logger;
		_personRepository = personRepository;
		_outputCacheStore = outputCacheStore;
	}

	public async Task Handle(PersonFromMediaRemoved notification, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Removing media with id {MediaId} from person with id {PersonId}", notification.MediaId.ToString(), 
			notification.PersonId.ToString());
		
		var person = await _personRepository.FindAsync(notification.PersonId, cancellationToken);
		ResultException.ThrowIfUnsuccessful(person);
		
		person.Value.RemoveMedia(notification.MediaId);
		await _outputCacheStore.EvictByTagAsync(notification.PersonId.ToString(), cancellationToken);
	}
}