using MediatR;
using Microsoft.AspNetCore.Http;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Commands.Platform;

public sealed record AddPlatformCommand(
	string Name, 
	decimal Price, 
	IFormFile Image
	) : IRequest<Result>;