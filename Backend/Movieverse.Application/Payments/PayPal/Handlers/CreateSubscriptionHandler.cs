using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Movieverse.Application.Common.Settings;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Payments.PayPal.Models;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Payments.PayPal.Requests;
using Movieverse.Domain.Common.Result;
using Newtonsoft.Json;

namespace Movieverse.Application.Payments.PayPal.Handlers;

public sealed class CreateSubscriptionHandler : IRequestHandler<CreateSubscriptionRequest, Result<string>>
{
	private readonly ILogger<CreateSubscriptionHandler> _logger;
	private readonly IHttpClientFactory _httpClientFactory;
	private readonly Common.Settings.PayPal _settings;
	private readonly IHttpService _httpService;
	private readonly IUserReadOnlyRepository _userReadOnlyRepository;

	public CreateSubscriptionHandler(ILogger<CreateSubscriptionHandler> logger, IHttpClientFactory httpClientFactory, 
		IOptions<PaymentsSettings> paymentsSettings, IHttpService httpService, IUserReadOnlyRepository userReadOnlyRepository)
	{
		_logger = logger;
		_httpClientFactory = httpClientFactory;
		_httpService = httpService;
		_userReadOnlyRepository = userReadOnlyRepository;
		_settings = paymentsSettings.Value.PayPal;
	}
	
	
	public async Task<Result<string>> Handle(CreateSubscriptionRequest request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Creating subscription...");
		try
		{
			var userId = _httpService.UserId;
			if (userId is null)
			{
				_logger.LogDebug("User is not logged in");
				return Error.Unauthorized(PaymentsResources.UserNotLoggedIn);
			}

			var userResult = await _userReadOnlyRepository.FindAsync(userId.Value, cancellationToken);
			if (userResult.IsUnsuccessful)
			{
				_logger.LogDebug("User not found");
				return Error.Unauthorized(PaymentsResources.UserNotLoggedIn);
			}
			var user = userResult.Value;
			
			var freeTrial = await _userReadOnlyRepository.IsFreeTrialAvailableAsync(userId.Value, cancellationToken);
			var paypalCreateSubscriptionRequest = JsonConvert.SerializeObject(new
			{
				plan_id = freeTrial ? _settings.TrialPlanId : _settings.PaidPlanId,
				subscriber = new
				{
					name = new
					{
						given_name = user.Information.FirstName,
						surname = user.Information.LastName
					},
					email_address = user.Email,
					shipping_address = new
					{
						name = new
						{
							full_name = $"{user.Information.FirstName} {user.Information.LastName}"
						},
						address = new
						{
							country_code = "US"
						}
					}
				}
			});
			
			using var httpClient = _httpClientFactory.CreateClient("PayPalClient");
			
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.PayPalAccessToken);
			
			var httpRequestMessage = new HttpRequestMessage
			{
				Content = new StringContent(paypalCreateSubscriptionRequest, Encoding.UTF8, "application/json"),
				Headers =
				{
					Accept =
					{
						new MediaTypeWithQualityHeaderValue("application/json")
					}
				}
			};
			
			var requestUri = $"{_settings.Url}{_settings.Endpoints.Subscriptions}";
			
			var response = await httpClient.PostAsync(requestUri, httpRequestMessage.Content, cancellationToken);
			var responseJson = await response.Content.ReadFromJsonAsync<CreatedSubscription>(cancellationToken: cancellationToken);

			if (responseJson is null)
			{
				_logger.LogDebug("Transaction failed. Response is null");
				return Error.Invalid(PaymentsResources.FailedToPay);
			}
			
			_logger.LogDebug("Created subscription with id: {id}", responseJson.id);
			return responseJson.id;
		}
		catch (Exception e)
		{
			_logger.LogDebug("Creating subscription failed. Exception: {msg}", e.Message);
			return Error.Invalid(PaymentsResources.FailedToCreateSubscription);
		}
	}
}