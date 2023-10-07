using MediatR;
using Microsoft.AspNetCore.Mvc;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Commands.User;

public sealed record UpdateWatchlistCommand(
	[FromRoute] Guid MediaId
	) : IRequest<Result>;