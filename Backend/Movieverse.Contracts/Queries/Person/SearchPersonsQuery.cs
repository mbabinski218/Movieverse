using MediatR;
using Movieverse.Contracts.DataTransferObjects.Person;
using Movieverse.Domain.Common;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Queries.Person;

public sealed record SearchPersonsQuery(	
	string? Term,
	short? PageNumber,
	short? PageSize
	) : IRequest<Result<IPaginatedList<SearchPersonDto>>>;