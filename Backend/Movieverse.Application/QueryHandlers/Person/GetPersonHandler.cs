using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Contracts.DataTransferObjects.Person;
using Movieverse.Contracts.Queries.Person;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.QueryHandlers.Person;

public sealed class GetPersonHandler : IRequestHandler<GetPersonQuery, Result<PersonDto>>
{
	private readonly ILogger<GetPersonHandler> _logger;
	private readonly IPersonReadOnlyRepository _personRepository;
	private readonly IMapper _mapper;


	public GetPersonHandler(ILogger<GetPersonHandler> logger, IPersonReadOnlyRepository personRepository, IMapper mapper)
	{
		_logger = logger;
		_personRepository = personRepository;
		_mapper = mapper;
	}

	public async Task<Result<PersonDto>> Handle(GetPersonQuery request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Getting person with id {Id}", request.Id);

		var person = await _personRepository.FindAsync(request.Id, cancellationToken).ConfigureAwait(false);
		if (person.IsUnsuccessful)
		{
			return person.Error;
		}
		
		var personDto = _mapper.Map<PersonDto>(person.Value);
		personDto.PictureIds = person.Value.ContentIds.Select(p => p.Value).ToList();

		return personDto;
	}
}