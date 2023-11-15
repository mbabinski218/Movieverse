using MediatR;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Commands.Person;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.DomainEvents;
using Movieverse.Domain.ValueObjects;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Application.CommandHandlers.PersonCommands.Update;

public sealed class UpdatePersonHandler : IRequestHandler<UpdatePersonCommand, Result>
{
	private readonly ILogger<UpdatePersonHandler> _logger;
	private readonly IPersonRepository _personRepository;
	private readonly IOutputCacheStore _outputCacheStore;
	private readonly IUnitOfWork _unitOfWork;

	public UpdatePersonHandler(ILogger<UpdatePersonHandler> logger, IPersonRepository personRepository, IOutputCacheStore outputCacheStore, 
		IUnitOfWork unitOfWork)
	{
		_logger = logger;
		_personRepository = personRepository;
		_outputCacheStore = outputCacheStore;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("UpdatePersonCommandHandler.Handle called");

		var personResult = await _personRepository.FindAsync(request.Id, cancellationToken);
		if (personResult.IsSuccessful)
		{
			return Error.NotFound(PersonResources.PersonDoesNotExist);
		}
		var person = personResult.Value;
		
		// Updating person
		if (request.Biography is not null) person.Biography = request.Biography;
		if (request.FunFacts is not null) person.FunFacts = request.FunFacts;
		if (request.Information is not null) person.Information = request.Information;
		if (request.LifeHistory is not null) person.LifeHistory = request.LifeHistory;
		
		// Updating picture
		if (request.Picture is not null)
		{
			var pictureId = person.PictureId ?? ContentId.Create();
			person.PictureId = pictureId;
			person.AddDomainEvent(new ImageChanged(pictureId, request.Picture));
		}
		
		// Adding pictures
		if(request.Pictures is not null)
		{
			foreach (var picture in request.Pictures)
			{
				var pictureId = ContentId.Create();
				person.AddContent(pictureId);
				person.AddDomainEvent(new ImageChanged(pictureId, picture));
			}
		}
		
		// Removing pictures
		if (request.PicturesToRemove is not null)
		{
			foreach (var contentId in request.PicturesToRemove)
			{
				person.RemoveContent(contentId);
			}
		}
		
		var updateResult = await _personRepository.UpdateAsync(person, cancellationToken);
		if (updateResult.IsUnsuccessful)
		{
			return updateResult.Error;
		}

		if (!await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false))
		{
			return Error.Invalid(PersonResources.CouldNotCreatePerson);
		}
		
		await _outputCacheStore.EvictByTagAsync(person.Id.ToString(), cancellationToken);
		return Result.Ok();
	}
}