using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Contracts.DataTransferObjects.User;
using Movieverse.Contracts.Queries;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.QueryHandlers.User;

public sealed class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
{
	private readonly ILogger<GetUserByIdHandler> _logger;
	private readonly IUserRepository _userRepository;
	private readonly IMapper _mapper;

	public GetUserByIdHandler(IUserRepository userRepository, IMapper mapper, ILogger<GetUserByIdHandler> logger)
	{
		_userRepository = userRepository;
		_mapper = mapper;
		_logger = logger;
	}

	public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Getting user {id}...", request.Id);
		
		var user = await _userRepository.FindByIdAsync(request.Id, cancellationToken);
		return user.IsSuccessful ? _mapper.Map<UserDto>(user.Value) : user.Error;
	}
}