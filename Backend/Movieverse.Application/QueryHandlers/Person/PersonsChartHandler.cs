using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.DataTransferObjects.Person;
using Movieverse.Contracts.Queries.Person;
using Movieverse.Domain.Common;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.QueryHandlers.Person;

public sealed class PersonsChartHandler : IRequestHandler<PersonsChartQuery, Result<IPaginatedList<SearchPersonDto>>>
{
	private readonly ILogger<PersonsChartHandler> _logger;
	private readonly IPersonReadOnlyRepository _personRepository;

	public PersonsChartHandler(ILogger<PersonsChartHandler> logger, IPersonReadOnlyRepository personRepository)
	{
		_logger = logger;
		_personRepository = personRepository;
	}

	public async Task<Result<IPaginatedList<SearchPersonDto>>> Handle(PersonsChartQuery request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Getting people born today...");
		
		return request.Category switch
		{
			"bornToday" => await _personRepository.FindPersonsBornTodayAsync(request.PageNumber, request.PageSize, cancellationToken),
			_ => Error.Invalid(PersonResources.InvalidChartCategory)
		};
	}
}
