using MediatR;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Commands.Person;

public sealed record CreatePersonCommand(bool ForUser) : IRequest<Result<string>>;