using System.Net.Http.Headers;
using System.Net.Http.Json;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Movieverse.Application.Common.Settings;
using Movieverse.Application.Payments.PayPal.Models;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Payments.PayPal.Requests;
using Movieverse.Contracts.Payments.PayPal.Responses;
using Movieverse.Domain.Common.Result;
using Newtonsoft.Json;

namespace Movieverse.Application.Payments.PayPal.Handlers;

public sealed class PlanHandler : IRequestHandler<PlanRequest, Result<PlanResponse>>
{
	private readonly ILogger<AuthorizationHandler> _logger;
	private readonly IHttpClientFactory _httpClientFactory;
	private readonly Common.Settings.PayPal _settings;

	public PlanHandler(ILogger<AuthorizationHandler> logger, IHttpClientFactory httpClientFactory, 
		IOptions<PaymentsSettings> paymentsSettings)
	{
		_logger = logger;
		_httpClientFactory = httpClientFactory;
		_settings = paymentsSettings.Value.PayPal;
	}
	
	public async Task<Result<PlanResponse>> Handle(PlanRequest request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Getting plan with id: {id}", _settings.PlanId);

		try
		{
			using var httpClient = _httpClientFactory.CreateClient("PayPalClient");
			
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.PayPalAccessToken);
			
			var requestUri = $"{_settings.Url}{_settings.Endpoints.Plans}/{_settings.PlanId}";
			var response = await httpClient.GetAsync(requestUri, cancellationToken);
			var responseJson = await response.Content.ReadFromJsonAsync<Plan>(cancellationToken: cancellationToken);
			
			var billing = responseJson?.billing_cycles.FirstOrDefault(x => x.tenure_type == "REGULAR");
			
			if (responseJson is null || billing is null)
			{
				_logger.LogDebug("Getting plan failed. Response is null");
				return Error.Invalid(PaymentsResources.FailedToPay); 
			}
			
			var plan = new PlanResponse
			{
				Name = responseJson.name,
				Description = responseJson.description,
				Price = billing.pricing_scheme.fixed_price.value,
				Currency = billing.pricing_scheme.fixed_price.currency_code
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