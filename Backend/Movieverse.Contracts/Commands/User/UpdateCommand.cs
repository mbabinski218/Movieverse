using MediatR;
using Microsoft.AspNetCore.Http;
using Movieverse.Contracts.DataTransferObjects.User;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Commands.User;

public sealed record UpdateCommand(
	string? UserName,
	string? Email,
	InformationDto? Information,
	IFormFile? Avatar
    ) : IRequest<Result<UserDto>>;