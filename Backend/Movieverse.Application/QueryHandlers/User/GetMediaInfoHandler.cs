using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.DataTransferObjects.User;
using Movieverse.Contracts.Queries.User;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.QueryHandlers.User;

public sealed class GetMediaInfoHandler : IRequestHandler<GetMediaInfoQuery, Result<MediaInfoDto>>
{
	private readonly ILogger<GetMediaInfoHandler> _logger;
	private readonly IUserReadOnlyRepository _userRepository;
	private readonly IHttpService _httpService;

	public GetMediaInfoHandler(ILogger<GetMediaInfoHandler> logger, IUserReadOnlyRepository userRepository, IHttpService httpService)
	{
		_logger = logger;
		_userRepository = userRepository;
		_httpService = httpService;
	}

	public async Task<Result<MediaInfoDto>> Handle(GetMediaInfoQuery request, CancellationToken cancellationToken)
	{
		var userId = _httpService.UserId;
		if (userId is null)
		{
			return Error.Unauthorized(UserResources.UserDoesNotExist);
		}
		
		_logger.LogDebug("Getting media info for user {UserId} and media {MediaId}", userId, request.MediaId);

		return await _userRepository.GetMediaInfoAsync(userId.Value, request.MediaId, cancellationToken);
	}
}