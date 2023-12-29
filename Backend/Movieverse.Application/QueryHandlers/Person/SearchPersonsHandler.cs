using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Contracts.DataTransferObjects.Person;
using Movieverse.Contracts.Queries.Person;
using Movieverse.Domain.Common;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.QueryHandlers.Person;

public sealed class SearchPersonsHandler : IRequestHandler<SearchPersonsQuery, Result<IPaginatedList<SearchPersonDto>>>
{
	private readonly ILogger<SearchPersonsHandler> _logger;
	private readonly IPersonReadOnlyRepository _personRepository;

	public SearchPersonsHandler(ILogger<SearchPersonsHandler> logger, IPersonReadOnlyRepository personRepository)
	{
		_logger = logger;
		_personRepository = personRepository;
	}

	public async Task<Result<IPaginatedList<SearchPersonDto>>> Handle(SearchPersonsQuery request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Searching people with term: {Term}", request.Term);

		return await _personRepository.SearchAsync(request.Term, request.PageNumber, request.PageSize, cancellationToken);
	}
}