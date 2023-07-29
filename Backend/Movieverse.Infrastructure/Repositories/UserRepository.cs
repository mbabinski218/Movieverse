using Movieverse.Application.Common.Result;
using Movieverse.Application.Interfaces;

namespace Movieverse.Infrastructure.Repositories;

public sealed class UserRepository : IUserRepository
{
	public async Task<Result> RegisterAsync()
	{
		return Error.NotImplemented();
	}
}