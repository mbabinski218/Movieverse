using MediatR;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Common.Exceptions;
using Movieverse.Application.Interfaces;
using Movieverse.Domain.DomainEvents;

namespace Movieverse.Application.EventHandlers;

public sealed class PersonToMediaAddedHandler : INotificationHandler<PersonToMediaAdded>
{
	private readonly ILogger<PersonToMediaAddedHandler> _logger;
	private readonly IPersonRepository _personRepository;
	private readonly IOutputCacheStore _outputCacheStore;

	public PersonToMediaAddedHandler(ILogger<PersonToMediaAddedHandler> logger, IPersonRepository personRepository, 
		IOutputCacheStore outputCacheStore)
	{
		_logger = logger;
		_personRepository = personRepository;
		_outputCacheStore = outputCacheStore;
	}

	public async Task Handle(PersonToMediaAdded notification, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Adding media with id {MediaId} to person with id {PersonId}", notification.MediaId.ToString(), 
			notification.PersonId.ToString());
		
		var person = await _personRepository.FindAsync(notification.PersonId, cancellationToken).ConfigureAwait(false);
		ResultException.ThrowIfUnsuccessful(person);
		
		person.Value.MediaIds.Add(notification.MediaId);
		await _outputCacheStore.EvictByTagAsync(notification.PersonId.ToString(), cancellationToken);
	}
}