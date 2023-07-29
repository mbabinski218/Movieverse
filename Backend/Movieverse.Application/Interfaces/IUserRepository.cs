using Movieverse.Application.Common.Result;

namespace Movieverse.Application.Interfaces;

public interface IUserRepository
{
	Task<Result> RegisterAsync();
}