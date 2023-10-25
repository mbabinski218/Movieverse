using MediatR;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Commands.User;

public sealed record UpdateRolesCommand(
	string Email,
	IEnumerable<string> Roles
	) : IRequest<Result>;