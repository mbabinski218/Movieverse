using MediatR;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Commands.Genre;

public sealed record AddGenreCommand(
	string Name, 
	string Description
	) : IRequest<Result>;