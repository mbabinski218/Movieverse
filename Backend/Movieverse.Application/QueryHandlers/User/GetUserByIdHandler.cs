using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.DataTransferObjects.User;
using Movieverse.Contracts.Queries.User;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.QueryHandlers.User;

public sealed class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
{
	private readonly ILogger<GetUserByIdHandler> _logger;
	private readonly IUserReadOnlyRepository _userRepository;
	private readonly IHttpService _httpService;
	
	public GetUserByIdHandler(ILogger<GetUserByIdHandler> logger, IUserReadOnlyRepository userRepository, IHttpService httpService)
	{
		_logger = logger;
		_userRepository = userRepository;
		_httpService = httpService;
	}

	public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
	{
		var id = _httpService.UserId;
		if (id is null)
		{
			_logger.LogDebug("User does not exist");
			return Error.Unauthorized(UserResources.UserDoesNotExist);
		}
		
		_logger.LogDebug("Getting user {id}...", id);
		
		var user = await _userRepository.FindAsync(id.Value, cancellationToken);
		return user.IsSuccessful ? user.Value : user.Error;
	}
}