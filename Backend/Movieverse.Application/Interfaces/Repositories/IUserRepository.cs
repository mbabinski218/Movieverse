using Movieverse.Contracts.DataTransferObjects.User;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Application.Interfaces.Repositories;

public interface IUserRepository
{
	Task<Result<User>> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
	Task<Result<User>> FindByEmailAsync(string email, CancellationToken cancellationToken = default);
	Task<Result<string>> GenerateEmailConfirmationTokenAsync(User user, CancellationToken cancellationToken = default);
	Task<Result<string>> RegisterAsync(User user, string? password, CancellationToken cancellationToken = default);
	Task<Result<TokensDto>> LoginAsync(User user, string password, CancellationToken cancellationToken = default);
	Task<Result<User>> FindByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
	Task<Result<TokensDto>> LoginWithRefreshTokenAsync(User user, string refreshToken, CancellationToken cancellationToken = default);
	Task<Result<TokensDto>> LoginWithGoogleAsync(string idToken, CancellationToken cancellationToken = default);
	Task<Result<TokensDto>> LoginWithFacebookAsync(string idToken, CancellationToken cancellationToken = default);
	Task<Result> ConfirmEmailAsync(User user, string token, CancellationToken cancellationToken = default);
	Task<Result> UpdateAsync(User user, CancellationToken cancellationToken = default);
	Task<Result> LogoutAsync(User user, CancellationToken cancellationToken = default);
	Task<Result<Information>> GetInformationAsync(Guid id, CancellationToken cancellationToken = default);
	Task<Result> AddPersonalityAsync(Guid id, PersonId personId, CancellationToken cancellationToken = default);
}