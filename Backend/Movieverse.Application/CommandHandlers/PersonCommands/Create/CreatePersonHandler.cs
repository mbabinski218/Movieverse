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

namespace Movieverse.Application.CommandHandlers.PersonCommands.Create;

public sealed class CreatePersonHandler : IRequestHandler<CreatePersonCommand, Result<string>>
{
	private readonly ILogger<CreatePersonCommand> _logger;
	private readonly IPersonRepository _personRepository;
	private readonly IUserReadOnlyRepository _userRepository;
	private readonly IHttpService _httpService;
	private readonly IUnitOfWork _unitOfWork;

	public CreatePersonHandler(ILogger<CreatePersonCommand> logger, IPersonRepository personRepository,  IUserReadOnlyRepository userRepository, 
		IHttpService httpService, IUnitOfWork unitOfWork)
	{
		_logger = logger;
		_personRepository = personRepository;
		_userRepository = userRepository;
		_httpService = httpService;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<string>> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("CreatePersonCommandHandler.Handle called");
		
		var personId = PersonId.Create();
		var person = Person.Create(personId);

		if (request.ForUser)
		{
			var userId = _httpService.UserId;
			if (userId is null)
			{
				return Error.Unauthorized(UserResources.YouAreNotLoggedIn);
			}
			
			var user = await _userRepository.FindAsync(userId.Value, cancellationToken);
			if (user.IsUnsuccessful)
			{
				return user.Error;
			}
			
			if (user.Value.PersonId is not null)
			{
				return Error.Invalid();
			}
			
			person.Information = new Information
			{
				FirstName = user.Value.Information.FirstName,
				LastName = user.Value.Information.LastName,
				Age = user.Value.Information.Age,
			};
			person.AddDomainEvent(new PersonalityCreated(personId, userId.Value));
		}
		
		var addResult = await _personRepository.AddAsync(person, cancellationToken);
		if (addResult.IsUnsuccessful)
		{
			return addResult.Error;
		}

		if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
		{
			return Error.Invalid(PersonResources.CouldNotCreatePerson);
		}
		
		return personId.ToString();
	}
}