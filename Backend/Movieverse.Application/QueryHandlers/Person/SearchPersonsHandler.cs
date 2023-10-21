using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Contracts.DataTransferObjects.Person;
using Movieverse.Contracts.Queries.Person;
using Movieverse.Domain.Common;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.QueryHandlers.Person;

public sealed class SearchPersonsHandler : IRequestHandler<SearchPersonsQuery, Result<IPaginatedList<SearchPersonDto>>>
{
	private readonly ILogger<SearchPersons> 
	
	public async Task<Result<IPaginatedList<SearchPersonDto>>> Handle(SearchPersonsQuery request, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
}