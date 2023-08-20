using MediatR;
using Movieverse.Contracts.DataTransferObjects.User;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Queries;

public sealed record GetUserByIdQuery(Guid Id) : IRequest<Result<UserDto>>;