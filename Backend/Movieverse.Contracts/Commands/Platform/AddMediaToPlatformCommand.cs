using MediatR;
using Microsoft.AspNetCore.Mvc;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Commands.Platform;

public sealed record AddMediaToPlatformCommand(
	[FromRoute] Guid Id, 
	Guid MediaId
	) : IRequest<Result>;