using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Payments.PayPal.Requests;
using Movieverse.Contracts.Payments.PayPal.Responses;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.Payments.PayPal.Handlers;

public sealed class GetSubscriptionHandler : IRequestHandler<GetSubscriptionRequest, Result<SubscriptionResponse>>
{
	private readonly ILogger<GetSubscriptionHandler> _logger;
	private readonly IUserReadOnlyRepository _userRepository;
	private readonly IHttpService _httpService;

	public GetSubscriptionHandler(ILogger<GetSubscriptionHandler> logger, IUserReadOnlyRepository userRepository, IHttpService httpService)
	{
		_logger = logger;
		_userRepository = userRepository;
		_httpService = httpService;
	}

	public async Task<Result<SubscriptionResponse>> Handle(GetSubscriptionRequest request, CancellationToken cancellationToken)
	{
		var userId = _httpService.UserId;
		if (userId is null)
		{
			return Error.Unauthorized(PaymentsResources.UserNotLoggedIn);
		}
		
		_logger.LogDebug("Get subscription for user: {UserId}", userId);
		
		return await _userRepository.GetSubscriptionAsync(userId.Value, cancellationToken);
	}
}