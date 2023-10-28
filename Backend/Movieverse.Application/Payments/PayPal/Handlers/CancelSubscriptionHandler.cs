using System.Net;
using System.Net.Http.Headers;
using System.Text;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Movieverse.Application.Common.Settings;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Payments.PayPal.Requests;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.Common.Types;
using Newtonsoft.Json;

namespace Movieverse.Application.Payments.PayPal.Handlers;

public sealed class CancelSubscriptionHandler : IRequestHandler<CancelSubscriptionRequest, Result>
{
	private readonly ILogger<CancelSubscriptionHandler> _logger;
	private readonly IHttpClientFactory _httpClientFactory;
	private readonly IUserRepository _userRepository;
	private readonly IHttpService _httpService;
	private readonly Common.Settings.PayPal _settings;
	private readonly IUnitOfWork _unitOfWork;

	public CancelSubscriptionHandler(ILogger<CancelSubscriptionHandler> logger, IHttpClientFactory httpClientFactory, 
		IUserRepository userRepository, IHttpService httpService, IUnitOfWork unitOfWork, IOptions<PaymentsSettings> paymentsSettings)
	{
		_logger = logger;
		_httpClientFactory = httpClientFactory;
		_userRepository = userRepository;
		_httpService = httpService;
		_unitOfWork = unitOfWork;
		_settings = paymentsSettings.Value.PayPal;
	}

	public async Task<Result> Handle(CancelSubscriptionRequest request, CancellationToken cancellationToken)
	{
		var userId = _httpService.UserId;
		if (userId is null)
		{
			_logger.LogDebug("User not logged in");
			return Error.Unauthorized(PaymentsResources.UserNotLoggedIn);
		}
		
		_logger.LogDebug("Cancel subscription for user: {UserId}", userId);
		
		var userResult = await _userRepository.FindByIdAsync(userId.Value, cancellationToken);
		if (userResult.IsUnsuccessful)
		{
			_logger.LogDebug("User not found");
			return Error.NotFound(PaymentsResources.UserNotLoggedIn);
		}
		var user = userResult.Value;

		try
		{
			using var httpClient = _httpClientFactory.CreateClient("PayPalClient"); 
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.PayPalAccessToken);
			
			var paypalCancelSubscriptionRequest = JsonConvert.SerializeObject(new
			{
				Reason = "User canceled subscription"
			});
			
			var httpRequestMessage = new HttpRequestMessage
			{
				Content = new StringContent(paypalCancelSubscriptionRequest, Encoding.UTF8, "application/json"),
				Headers = 
				{
					Accept =
					{
						new MediaTypeWithQualityHeaderValue("application/json")
					}
				}
			};
			
			var requestUri = $"{_settings.Url}{_settings.Endpoints.Subscriptions}/{user.Subscription.Id}/cancel";
			var response = await httpClient.PostAsync(requestUri, httpRequestMessage.Content, cancellationToken);

			if (response.StatusCode != HttpStatusCode.NoContent)
			{
				_logger.LogDebug("Transaction failed. Response is null");
				return Error.Invalid(PaymentsResources.CancelSubscriptionError);
			}
		}
		catch (Exception e)
		{
			_logger.LogError("Error while canceling subscription: {msg}", e.Message);
			return Error.InternalError(PaymentsResources.CancelSubscriptionError);
		}

		var removeRoleResult = await _userRepository.RemoveRoleAsync(user, UserRole.Pro.ToStringFast(), cancellationToken);
		if (removeRoleResult.IsUnsuccessful)
		{
			return Error.InternalError(PaymentsResources.CancelSubscriptionError);
		}
		
		user.Subscription.Id = null;
		user.Subscription.Since = null;
		
		var updateResult = await _userRepository.UpdateAsync(user, cancellationToken);
		if (updateResult.IsUnsuccessful)
		{
			return Error.InternalError(PaymentsResources.CancelSubscriptionError);
		}
		
		if (!await _unitOfWork.SaveChangesAsync(cancellationToken))
		{
			return Error.InternalError(PaymentsResources.CancelSubscriptionError);
		}
		
		return Result.Ok();
	}
}