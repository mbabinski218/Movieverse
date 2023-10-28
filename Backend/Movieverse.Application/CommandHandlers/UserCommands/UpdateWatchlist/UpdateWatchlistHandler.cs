using MediatR;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Commands.User;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.Entities;

namespace Movieverse.Application.CommandHandlers.UserCommands.UpdateWatchlist;

public sealed class UpdateWatchlistHandler : IRequestHandler<UpdateWatchlistCommand, Result>
{
	private readonly ILogger<UpdateWatchlistHandler> _logger;
	private readonly IUserRepository _userRepository;
	private readonly IOutputCacheStore _outputCacheStore;
	private readonly IHttpService _httpService;
	private readonly IUnitOfWork _unitOfWork;

	public UpdateWatchlistHandler(ILogger<UpdateWatchlistHandler> logger, IUserRepository userRepository, IOutputCacheStore outputCacheStore, 
		IHttpService httpService, IUnitOfWork unitOfWork)
	{
		_logger = logger;
		_userRepository = userRepository;
		_outputCacheStore = outputCacheStore;
		_httpService = httpService;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result> Handle(UpdateWatchlistCommand request, CancellationToken cancellationToken)
	{
		var userId = _httpService.UserId;
		if (userId is null)
		{
			return Error.Unauthorized(UserResources.YouAreNotLoggedIn);
		}
		_logger.LogDebug("User with id {UserId} is updating their watchlist", userId);
		
		var mediaInfoResult = await _userRepository.FindMediaInfoAsync(userId.Value, request.MediaId, cancellationToken);
		if (mediaInfoResult.IsUnsuccessful)
		{
			return mediaInfoResult.Error;
		}

		var mediaInfo = mediaInfoResult.Value;
		if (mediaInfo is null)
		{
			var userResult = await _userRepository.FindByIdAsync(userId.Value, cancellationToken);
			if(userResult.IsUnsuccessful)
			{
				return userResult.Error;
			}

			var user = userResult.Value;
			
			var newMediaInfo = MediaInfo.Create(user, request.MediaId, true, 0);
			user.AddMediaInfo(newMediaInfo);
		}
		else
		{
			mediaInfo.IsOnWatchlist = !mediaInfo.IsOnWatchlist;
		}

		if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
		{
			_logger.LogError("Failed to update watchlist for user with id {UserId}", userId);
			return Error.Invalid(UserResources.UserUpdateFailed);
		}
		
		await _outputCacheStore.EvictByTagAsync(userId.ToString()!, cancellationToken);
		return Result.Ok();
	}
}