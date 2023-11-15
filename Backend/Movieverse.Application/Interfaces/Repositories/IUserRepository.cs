using Movieverse.Contracts.DataTransferObjects.User;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.Entities;
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
	Task<Result<MediaInfo?>> FindMediaInfoAsync(Guid id, MediaId mediaId, CancellationToken cancellationToken = default);
	Task<Result> ChangeUsernameAsync(User user, string username, CancellationToken cancellationToken = default);
	Task<Result> ChangeEmailAsync(User user, string email, CancellationToken cancellationToken = default);
	Task<Result> UpdateRolesAsync(User user, IList<string> roles, CancellationToken cancellationToken = default);
	Task<Result> AddRoleAsync(User user, string role, CancellationToken cancellationToken = default);
	Task<Result> RemoveRoleAsync(User user, string role, CancellationToken cancellationToken = default);
	Task<bool> IsSystemAdministratorAsync(User user, CancellationToken cancellationToken = default);
	Task<Result> ChangePasswordAsync(User user, string currentPassword, string newPassword, CancellationToken cancellationToken = default);
}