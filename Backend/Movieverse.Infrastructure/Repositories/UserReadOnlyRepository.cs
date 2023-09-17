using MapsterMapper;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.DataTransferObjects.User;
using Movieverse.Domain.Common.Result;
using Movieverse.Infrastructure.Persistence;

namespace Movieverse.Infrastructure.Repositories;

public sealed class UserReadOnlyRepository : IUserReadOnlyRepository
{
	private readonly ILogger<UserReadOnlyRepository> _logger;
	private readonly ReadOnlyContext _dbContext;
	private readonly IMapper _mapper;

	public UserReadOnlyRepository(ILogger<UserReadOnlyRepository> logger, ReadOnlyContext dbContext, IMapper mapper)
	{
		_logger = logger;
		_dbContext = dbContext;
		_mapper = mapper;
	}
	public async Task<Result<UserDto>> FindAsync(Guid id, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Find user with id: {id}", id);
		
		var user = await _dbContext.Users.FindAsync(new object?[] { id }, cancellationToken).ConfigureAwait(false);
		return user is null ? Error.NotFound(UserResources.UserDoesNotExist) : _mapper.Map<UserDto>(user);
	}
}