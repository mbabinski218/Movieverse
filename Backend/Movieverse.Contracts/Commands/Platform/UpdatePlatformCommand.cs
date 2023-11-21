using MediatR;
using Microsoft.AspNetCore.Mvc;
using Movieverse.Contracts.DataTransferObjects.Platform;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Commands.Platform;

public sealed record UpdatePlatformCommand(
	[FromRoute] Guid Id,
	string? Name, 
	decimal? Price
	) : IRequest<Result<PlatformDto>>;