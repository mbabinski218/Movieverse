using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Contracts.Queries.User;
using Movieverse.Domain.Common;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.QueryHandlers.User;

public sealed class GetWatchlistHandler : IRequestHandler<GetWatchlistQuery, Result<IPaginatedList<SearchMediaDto>>>
{
	private readonly ILogger<GetWatchlistHandler> _logger;
	private readonly IHttpService _httpService;
    private readonly IUserReadOnlyRepository _userRepository;
	private readonly IMediaReadOnlyRepository _mediaRepository;	

	public GetWatchlistHandler(ILogger<GetWatchlistHandler> logger, IMediaReadOnlyRepository mediaRepository, IHttpService httpService, 
		IUserReadOnlyRepository userRepository)
	{
		_logger = logger;
		_mediaRepository = mediaRepository;
		_httpService = httpService;
		_userRepository = userRepository;
	}

	public async Task<Result<IPaginatedList<SearchMediaDto>>> Handle(GetWatchlistQuery request, CancellationToken cancellationToken)
	{
		var userId = _httpService.UserId;
		if (userId is null)
		{
			return Error.Unauthorized(UserResources.UserDoesNotExist);
		}
		
		_logger.LogDebug("Getting watchlist for user with id {UserId}", userId);
		
		var watchlistIds = await _userRepository.GetWatchlistAsync(userId.Value, cancellationToken);
		if (watchlistIds.IsUnsuccessful)
		{
			return watchlistIds.Error;
		}
		
		var watchlist = await _mediaRepository.FindMediaByIdsAsync(watchlistIds.Value.ToList(), request.PageNumber, request.PageSize, cancellationToken);
		return watchlist;
	}
}