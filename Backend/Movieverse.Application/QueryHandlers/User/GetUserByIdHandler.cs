using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Contracts.DataTransferObjects.User;
using Movieverse.Contracts.Queries.User;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.QueryHandlers.User;

public sealed class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
{
	private readonly ILogger<GetUserByIdHandler> _logger;
	private readonly IUserReadOnlyRepository _userRepository;

	public GetUserByIdHandler(ILogger<GetUserByIdHandler> logger, IUserReadOnlyRepository userRepository)
	{
		_logger = logger;
		_userRepository = userRepository;
	}

	public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Getting user {id}...", request.Id);
		
		var user = await _userRepository.FindAsync(request.Id, cancellationToken);
		return user.IsSuccessful ? user.Value : user.Error;
	}
}