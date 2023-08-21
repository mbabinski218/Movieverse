using MapsterMapper;
using MediatR;
using Movieverse.Application.Interfaces;
using Movieverse.Contracts.DataTransferObjects.User;
using Movieverse.Contracts.Queries;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.QueryHandlers.User;

public sealed class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
{
	private readonly IUserRepository _userRepository;
	private readonly IMapper _mapper;

	public GetUserByIdHandler(IUserRepository userRepository, IMapper mapper)
	{
		_userRepository = userRepository;
		_mapper = mapper;
	}

	public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
	{
		var user = await _userRepository.FindByIdAsync(request.Id, cancellationToken);

		return user.IsSuccessful ? _mapper.Map<UserDto>(user.Value) : user.Error;
	}
}