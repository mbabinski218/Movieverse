using MediatR;
using Movieverse.Contracts.DataTransferObjects.User;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Contracts.Commands.User;

public sealed record LoginCommand(
	string GrantType, 
	string? Email, 
	string? Password, 
	string? RefreshToken,
	string? IdToken
	) : IRequest<Result<TokensDto>>;
