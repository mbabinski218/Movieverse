using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.DataTransferObjects.User;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;
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
		_logger.LogDebug("Database - Find user with id: {id}", id);

		var user = await _dbContext.Users.FindAsync(new object?[] { id }, cancellationToken);
		return user is null ? Error.NotFound(UserResources.UserDoesNotExist) : _mapper.Map<UserDto>(user);
	}

	public async Task<Result<IEnumerable<WatchlistStatusDto>>> GetWatchlistStatusesAsync(Guid userId, IEnumerable<MediaId> mediaIds, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - Get watchlist statuses for user with id: {UserId}", userId);
		
		var statuses = await _dbContext.Users
			.AsNoTracking()
			.Where(x => x.Id == userId)
			.SelectMany(x => x.MediaInfos)
			.ProjectToType<WatchlistStatusDto>()
			.ToListAsync(cancellationToken);

		var mediaIdsList = mediaIds.ToList();
		var watchlistStatuses = statuses
			.Where(x => mediaIdsList.Any(y => y.Value == x.MediaId))
			.ToList();

		return watchlistStatuses;
	}

	public async Task<Result<IEnumerable<MediaId>>> GetWatchlistAsync(Guid userId, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Database - get watchlist for user with id: {UserId}", userId);
		
		var watchlist = await _dbContext.Users
			.AsNoTracking()
			.Where(x => x.Id == userId)
			.SelectMany(x => x.MediaInfos)
			.Where(x => x.IsOnWatchlist)
			.Select(x => x.MediaId)
			.ToListAsync(cancellationToken);

		return watchlist;
	}
}