using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Movieverse.Application.Common.Settings;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Payments.PayPal.Models;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Payments.PayPal.Requests;
using Movieverse.Contracts.Payments.PayPal.Responses;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.Payments.PayPal.Handlers;

public sealed class PlanHandler : IRequestHandler<PlanRequest, Result<PlanResponse>>
{
	private readonly ILogger<AuthorizationHandler> _logger;
	private readonly IHttpClientFactory _httpClientFactory;
	private readonly Common.Settings.PayPal _settings;
	private readonly IUserReadOnlyRepository _userRepository;
	private readonly IHttpService _httpService;

	public PlanHandler(ILogger<AuthorizationHandler> logger, IHttpClientFactory httpClientFactory, 
		IOptions<PaymentsSettings> paymentsSettings, IUserReadOnlyRepository userRepository, IHttpService httpService)
	{
		_logger = logger;
		_httpClientFactory = httpClientFactory;
		_userRepository = userRepository;
		_httpService = httpService;
		_settings = paymentsSettings.Value.PayPal;
	}
	
	public async Task<Result<PlanResponse>> Handle(PlanRequest request, CancellationToken cancellationToken)
	{
		var userId = _httpService.UserId;
		if (userId is null)
		{
			_logger.LogDebug("User not logged in");
			return Error.Unauthorized(PaymentsResources.UserNotLoggedIn);
		}
		
		_logger.LogDebug("Getting plan info for user: {UserId}", userId);
		
		try
		{
			using var httpClient = _httpClientFactory.CreateClient("PayPalClient");
			
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.PayPalAccessToken);
			
			var freeTrial = await _userRepository.IsFreeTrialAvailableAsync(userId.Value, cancellationToken);
			var planId = freeTrial ? _settings.TrialPlanId : _settings.PaidPlanId;
			
			var requestUri = $"{_settings.Url}{_settings.Endpoints.Plans}/{planId}";
			var response = await httpClient.GetAsync(requestUri, cancellationToken);
			var responseJson = await response.Content.ReadFromJsonAsync<Plan>(cancellationToken: cancellationToken);
			
			var billing = responseJson?.billing_cycles.FirstOrDefault(x => x.tenure_type == "REGULAR");
			
			if (responseJson is null || billing is null)
			{
				_logger.LogDebug("Getting plan failed. Response is null");
				return Error.Invalid(PaymentsResources.FailedToPay); 
			}
			
			var price = Convert.ToDouble(billing.pricing_scheme.fixed_price.value);
			var tax = Convert.ToDouble(responseJson.taxes.percentage);
			var totalPrice = price + (price * tax / 100);
			var fixedPrice = Math.Round(totalPrice, 2);
			
			var plan = new PlanResponse
			{
				Name = responseJson.name,
				Description = responseJson.description,
				Price = fixedPrice.ToString(CultureInfo.InvariantCulture),
				Currency = billing.pricing_scheme.fixed_price.currency_code,
				FreeTrial = freeTrial
			};

			_logger.LogDebug("Getting plan with id completed");
			return plan;
		}
		catch (Exception e)
		{
			_logger.LogError(e, "Failed to get plan: {msg}", e.Message);
			return Error.Invalid(PaymentsResources.FailedToGetPlan);
		}
	}
}