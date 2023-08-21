using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.Application.Interfaces;

public interface IUserRepository
{
	Task<Result<User>> FindByIdAsync(AggregateRootId id, CancellationToken cancellationToken);
	Task<Result<User>> FindByEmailAsync(string email, CancellationToken cancellationToken);
	Task<Result<string>> GenerateEmailConfirmationTokenAsync(User user, CancellationToken cancellationToken);
	Task<Result<string>> RegisterAsync(User user, string password, CancellationToken cancellationToken);
	Task<Result> ConfirmEmailAsync(User user, string token, CancellationToken cancellationToken);
	Task<Result> UpdateAsync(User user, CancellationToken cancellationToken);
	Task<Result> Test();
}