using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Payments.PayPal.Requests;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.Common.Types;

namespace Movieverse.Application.Payments.PayPal.Handlers;

public sealed class StartSubscriptionHandler : IRequestHandler<StartSubscriptionRequest, Result>
{
	private readonly ILogger<StartSubscriptionHandler> _logger;
	private readonly IUserRepository _userRepository;
	private readonly IHttpService _httpService;
	private readonly IUnitOfWork _unitOfWork;

	public StartSubscriptionHandler(ILogger<StartSubscriptionHandler> logger, IUserRepository userRepository, IHttpService httpService, 
		IUnitOfWork unitOfWork)
	{
		_logger = logger;
		_userRepository = userRepository;
		_httpService = httpService;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result> Handle(StartSubscriptionRequest request, CancellationToken cancellationToken)
	{
		var userId = _httpService.UserId;
		if (userId is null)
		{
			return Error.Unauthorized(PaymentsResources.UserNotLoggedIn);
		}
		
		_logger.LogDebug("Start subscription: {SubId} for user: {UserId}", request.SubscriptionId, userId);
		
		var userResult = await _userRepository.FindByIdAsync(userId.Value, cancellationToken);
		if (userResult.IsUnsuccessful)
		{
			return Error.NotFound(PaymentsResources.UserNotLoggedIn);
		}
		var user = userResult.Value;

		var addRoleResult = await _userRepository.AddRoleAsync(user, UserRole.Pro.ToStringFast(), cancellationToken);
		if (addRoleResult.IsUnsuccessful)
		{
			return Error.InternalError(PaymentsResources.StartSubscriptionError);
		}
		
		user.Subscription.FreeTrial = false;
		user.Subscription.Id = request.SubscriptionId;
		user.Subscription.Since = DateTime.UtcNow.Date;
		
		var updateResult = await _userRepository.UpdateAsync(user, cancellationToken);
		if (updateResult.IsUnsuccessful)
		{
			await _userRepository.RemoveRoleAsync(user, UserRole.Pro.ToStringFast(), cancellationToken);
			return Error.InternalError(PaymentsResources.StartSubscriptionError);
		}

		if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
		{
			await _userRepository.RemoveRoleAsync(user, UserRole.Pro.ToStringFast(), cancellationToken);
			return Error.InternalError(PaymentsResources.StartSubscriptionError);
		}

		return Result.Ok();
	}
}