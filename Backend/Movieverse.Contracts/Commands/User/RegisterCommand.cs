using MediatR;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Commands.User;

public sealed record RegisterCommand(
	string Email,
	string UserName,
	string? FirstName,
	string? LastName,
	string Password,
	string ConfirmPassword,
	short Age
	) : IRequest<Result>;