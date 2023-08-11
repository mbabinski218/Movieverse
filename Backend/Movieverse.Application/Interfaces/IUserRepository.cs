using Movieverse.Application.Common.Result;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.Application.Interfaces;

public interface IUserRepository
{
	Task<Result<User>> FindByIdAsync(AggregateRootId id);
	Task<Result<User>> FindByEmailAsync(string email);
	Task<Result<string>> GenerateEmailConfirmationTokenAsync(User user);
	Task<Result<string>> RegisterAsync(User user, string password);
	Task<Result> ConfirmEmailAsync(User user, string token);
}