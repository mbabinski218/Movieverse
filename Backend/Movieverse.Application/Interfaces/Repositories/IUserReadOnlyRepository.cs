using Movieverse.Contracts.DataTransferObjects.User;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.Interfaces.Repositories;

public interface IUserReadOnlyRepository
{
	Task<Result<UserDto>> FindAsync(Guid id, CancellationToken cancellationToken = default);
}