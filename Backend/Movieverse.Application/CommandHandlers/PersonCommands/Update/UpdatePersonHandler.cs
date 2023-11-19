using MediatR;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Commands.Person;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.DomainEvents;
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
		_logger.LogDebug("Updating person with id {Id}", request.Id);

		var personResult = await _personRepository.FindAsync(request.Id, cancellationToken);
		if (personResult.IsUnsuccessful)
		{
			return personResult.Error;
		}
		var person = personResult.Value;
		
		// Updating person
		person.Biography = request.Biography;
		person.FunFacts = request.FunFacts;

		person.Information = request.Information ?? new();
		person.LifeHistory = request.LifeHistory ?? new();
		person.LifeHistory.BirthDate = person.LifeHistory.BirthDate?.UtcDateTime;
		person.LifeHistory.DeathDate = person.LifeHistory.DeathDate?.UtcDateTime;
		person.LifeHistory.CareerStart = person.LifeHistory.CareerStart?.UtcDateTime;
		person.LifeHistory.CareerEnd = person.LifeHistory.CareerEnd?.UtcDateTime;

		// Updating picture
		if (request.ChangePicture ?? false)
		{
			if (request.Picture is not null)
			{
				var pictureId = person.PictureId ?? ContentId.Create();
				person.PictureId = pictureId;
				person.AddDomainEvent(new ImageChanged(pictureId, request.Picture));
			}
			else
			{
				person.PictureId = null;
			}
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
		
		// Database operations
		var updateResult = await _personRepository.UpdateAsync(person, cancellationToken);
		if (updateResult.IsUnsuccessful)
		{
			return updateResult.Error;
		}

		if (!await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false))
		{
			return Error.Invalid(PersonResources.CouldNotCreatePerson);
		}
		
		// Evicting cache
		await _outputCacheStore.EvictByTagAsync(person.Id.ToString(), cancellationToken);
		return Result.Ok();
	}
}