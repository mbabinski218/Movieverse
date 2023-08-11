using MediatR;
using Movieverse.Application.Common.Result;

namespace Movieverse.Application.Commands.UserCommands.ResendEmailConfirmation;

public sealed record ResendEmailConfirmationCommand(string Email) : IRequest<Result>;