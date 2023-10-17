using MediatR;
using Movieverse.Contracts.DataTransferObjects.User;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Queries.User;

public sealed record GetUserByIdQuery : IRequest<Result<UserDto>>;