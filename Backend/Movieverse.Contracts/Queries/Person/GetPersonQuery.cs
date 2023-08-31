using MediatR;
using Movieverse.Contracts.DataTransferObjects.Person;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Queries.Person;

public sealed record GetPersonQuery(Guid Id) : IRequest<Result<PersonDto>>;