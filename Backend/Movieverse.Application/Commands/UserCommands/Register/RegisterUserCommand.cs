using MediatR;
using Movieverse.Application.Common.Result;

namespace Movieverse.Application.Commands.UserCommands.Register;

public sealed record RegisterUserCommand(
	string Email,
	string UserName,
	string? FirstName,
	string? LastName,
	string Password,
	string ConfirmPassword,
	short Age
	) : IRequest<Result>;