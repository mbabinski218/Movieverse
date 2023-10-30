using MediatR;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Commands.User;

public sealed record UpdateRatingCommand(
	Guid MediaId,
	ushort Rating
	) : IRequest<Result>;