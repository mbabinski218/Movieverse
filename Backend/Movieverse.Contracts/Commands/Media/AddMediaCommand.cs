using MediatR;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Commands.Media;

public sealed record AddMediaCommand(
	string Type,
	string Title
	) : IRequest<Result<string>>;