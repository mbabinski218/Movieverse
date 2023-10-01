using MediatR;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Commands.User;

public sealed record ConfirmEmailCommand(
	Guid Id, 
	string Token) 
	: IRequest<Result>;