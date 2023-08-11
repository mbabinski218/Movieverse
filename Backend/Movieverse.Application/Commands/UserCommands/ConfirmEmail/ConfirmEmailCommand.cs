using MediatR;
using Movieverse.Application.Common.Result;

namespace Movieverse.Application.Commands.UserCommands.ConfirmEmail;

public sealed record ConfirmEmailCommand(
	Guid Id, 
	string Token) 
	: IRequest<Result>;