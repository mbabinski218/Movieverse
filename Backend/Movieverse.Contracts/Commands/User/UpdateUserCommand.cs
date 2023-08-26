using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movieverse.Contracts.DataTransferObjects.User;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Commands.User;

public sealed record UpdateUserCommand(
	[FromRoute] Guid Id,
	string? UserName,
	string? Email,
	InformationDto? Information,
	IFormFile? Avatar
    ) : IRequest<Result<UserDto>>;