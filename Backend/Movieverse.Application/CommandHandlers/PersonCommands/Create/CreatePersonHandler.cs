using MediatR;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Commands.Person;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.DomainEvents;
using Movieverse.Domain.ValueObjects;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Application.CommandHandlers.PersonCommands.Create;

public sealed class CreatePersonHandler : IRequestHandler<CreatePersonCommand, Result>
{
	private readonly ILogger<CreatePersonCommand> _logger;
	private readonly IPersonRepository _personRepository;
	private readonly IUserRepository _userRepository;
	private readonly IOutputCacheStore _outputCacheStore;
	private readonly IHttpService _httpService;
	private readonly IUnitOfWork _unitOfWork;

	public CreatePersonHandler(ILogger<CreatePersonCommand> logger, IPersonRepository personRepository,  IUserRepository userRepository, 
		IOutputCacheStore outputCacheStore, IHttpService httpService, IUnitOfWork unitOfWork)
	{
		_logger = logger;
		_personRepository = personRepository;
		_userRepository = userRepository;
		_outputCacheStore = outputCacheStore;
		_httpService = httpService;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("CreatePersonCommandHandler.Handle called");
		
		var userId = _httpService.UserId;
		if (userId is null)
		{
			return Error.Unauthorized(UserResources.YouAreNotLoggedIn);
		}

		Information information;
		switch (request.Information)
		{
			case null when request.ForUser:
			{
				var informationResult = await _userRepository.GetInformationAsync(userId.Value, cancellationToken).ConfigureAwait(false);
				if (informationResult.IsUnsuccessful)
				{
					return informationResult.Error;
				}
				information = informationResult.Value;
				break;
			}
			case null:
				return Error.Invalid(PlatformResources.InformationCannotBeNull);
			default:
				information = request.Information;
				break;
		}
		
		var personId = PersonId.Create();
		var person = Person.Create(
			personId, 
			information, 
			request.LifeHistory, 
			request.Biography, 
			request.FunFacts);

		if (request.ForUser)
		{
			person.AddDomainEvent(new PersonalityCreated(personId, userId.Value));
		}
		
		// Adding pictures
		if(request.Pictures is not null)
		{
			foreach (var picture in request.Pictures)
			{
				var pictureId = ContentId.Create();
				person.AddDomainEvent(new ImageChanged(pictureId, picture));
				person.ContentIds.Add(pictureId);
			}
		}
		
		var addResult = await _personRepository.AddAsync(person, cancellationToken).ConfigureAwait(false);
		if (addResult.IsUnsuccessful)
		{
			return addResult.Error;
		}

		if (!await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false))
		{
			return Error.Invalid(PersonResources.CouldNotCreatePerson);
		}
		
		await _outputCacheStore.EvictByTagAsync(personId.ToString(), cancellationToken).ConfigureAwait(false);
		return Result.Ok();
	}
}