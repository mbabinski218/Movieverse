using Movieverse.Application.Common.Result;
using Movieverse.Domain.AggregateRoots;

namespace Movieverse.Application.Interfaces;

public interface IUserRepository
{
	Task<Result> RegisterAsync(User user, string password);
}