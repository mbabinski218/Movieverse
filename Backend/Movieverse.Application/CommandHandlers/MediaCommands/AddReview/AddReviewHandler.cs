using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Commands.Media;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.Entities;

namespace Movieverse.Application.CommandHandlers.MediaCommands.AddReview;

public sealed class AddReviewHandler : IRequestHandler<AddReviewCommand, Result<ReviewDto>>
{
	private readonly ILogger<AddReviewHandler> _logger;
	private readonly IMediaRepository _mediaRepository;
	private readonly IUserReadOnlyRepository _userRepository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IHttpService _httpService;
	private readonly IMapper _mapper;

	public AddReviewHandler(ILogger<AddReviewHandler> logger, IMediaRepository mediaRepository, IUnitOfWork unitOfWork, 
		IHttpService httpService, IUserReadOnlyRepository userRepository, IMapper mapper)
	{
		_logger = logger;
		_mediaRepository = mediaRepository;
		_unitOfWork = unitOfWork;
		_httpService = httpService;
		_userRepository = userRepository;
		_mapper = mapper;
	}

	public async Task<Result<ReviewDto>> Handle(AddReviewCommand request, CancellationToken cancellationToken)
	{
		var userId = _httpService.UserId;
		if (userId is null)
		{
			return Error.Unauthorized();
		}
		
		_logger.LogDebug("AddReviewCommandHandler.Handle - Adding review for media with id {Id}", userId);
		
		var userResult = await _userRepository.FindAsync(userId.Value, cancellationToken);
		if (userResult.IsUnsuccessful)
		{
			return userResult.Error;
		}
		var user = userResult.Value;

		if (user.Banned)
		{
			return Error.Invalid();
		}
		
		var userName = user.Information.FirstName is not null && user.Information.LastName is not null
			? $"{user.Information.FirstName} {user.Information.LastName}"
			: user.UserName;
		
		var media = await _mediaRepository.FindAsync(request.Id, cancellationToken);
		if (media.IsUnsuccessful)
		{
			return media.Error;
		}

		var review = Review.Create(media.Value, userId.Value, userName, request.Text);
		media.Value.AddReview(review);

		media.Value.BasicStatistics.ReviewCount++;
		
		await _mediaRepository.UpdateAsync(media.Value, cancellationToken);
		
		if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
		{
			return Error.Invalid(MediaResources.CouldNotUpdateMedia);
		}

		return _mapper.Map<ReviewDto>(review);
	}
}