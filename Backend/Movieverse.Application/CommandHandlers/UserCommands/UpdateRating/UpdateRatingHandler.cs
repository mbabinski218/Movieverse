using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Commands.User;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.Entities;

namespace Movieverse.Application.CommandHandlers.UserCommands.UpdateRating;

public sealed class UpdateRatingHandler : IRequestHandler<UpdateRatingCommand, Result>
{
	private readonly ILogger<UpdateRatingHandler> _logger;
	private readonly IUserRepository _userRepository;
	private readonly IHttpService _httpService;
	private readonly IUnitOfWork _unitOfWork;

	public UpdateRatingHandler(ILogger<UpdateRatingHandler> logger, IUserRepository userRepository, IHttpService httpService, 
		IUnitOfWork unitOfWork)
	{
		_logger = logger;
		_userRepository = userRepository;
		_httpService = httpService;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result> Handle(UpdateRatingCommand request, CancellationToken cancellationToken)
	{
		var userId= _httpService.UserId;
		if (userId is null)
		{
			return Error.Unauthorized(UserResources.UserDoesNotExist);
		}
		
		_logger.LogDebug("User with id {userId} updating rating for media with id {MediaId}", userId, request.MediaId.ToString());
		
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
			
			var newMediaInfo = MediaInfo.Create(user, request.MediaId, false, request.Rating);
			user.AddMediaInfo(newMediaInfo);
		}
		else
		{
			if (mediaInfo.Rating == request.Rating)
			{
				return Result.Ok();
			}
			
			mediaInfo.Rating = request.Rating;
		}

		if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
		{
			_logger.LogError("Failed to update rating for user with id {UserId}", userId);
			return Error.Invalid(UserResources.UserUpdateFailed);
		}
		
		return Result.Ok();
	}
}