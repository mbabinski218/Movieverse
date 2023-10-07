using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.DataTransferObjects.User;
using Movieverse.Contracts.Queries.User;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Application.QueryHandlers.User;

public sealed class GetWatchlistStatusesHandler : IRequestHandler<GetWatchlistStatusesQuery, Result<IEnumerable<WatchlistStatusDto>>>
{
	private readonly ILogger<GetWatchlistStatusesHandler> _logger;
	private readonly IUserReadOnlyRepository _userRepository;
	private readonly IHttpService _httpService;

	public GetWatchlistStatusesHandler(ILogger<GetWatchlistStatusesHandler> logger, IUserReadOnlyRepository userRepository, IHttpService httpService)
	{
		_logger = logger;
		_userRepository = userRepository;
		_httpService = httpService;
	}

	public async Task<Result<IEnumerable<WatchlistStatusDto>>> Handle(GetWatchlistStatusesQuery request, CancellationToken cancellationToken)
	{
		var userId = _httpService.UserId;
		if (userId is null)
		{
			return Error.Unauthorized(UserResources.YouAreNotLoggedIn);
		}
		_logger.LogDebug("User with id {UserId} is getting their watchlist statuses", userId);
		
		return await _userRepository.GetWatchlistStatusesAsync(userId.Value, request.MediaIds.Select(MediaId.Create), cancellationToken);
	}
}