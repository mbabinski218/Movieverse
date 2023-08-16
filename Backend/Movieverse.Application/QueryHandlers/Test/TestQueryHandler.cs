using MediatR;
using Movieverse.Application.Interfaces;
using Movieverse.Contracts.Queries;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.QueryHandlers.Test;

public sealed class TestQueryHandler : IRequestHandler<TestQuery, Result>
{
	private readonly IUserRepository _userRepository;

	public TestQueryHandler(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	public async Task<Result> Handle(TestQuery request, CancellationToken cancellationToken)
	{
		return await _userRepository.Test();
	}
}