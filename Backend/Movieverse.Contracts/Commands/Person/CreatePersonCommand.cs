using MediatR;
using Microsoft.AspNetCore.Http;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects;

namespace Movieverse.Contracts.Commands.Person;

public sealed record CreatePersonCommand(bool ForUser) : IRequest<Result<string>>;