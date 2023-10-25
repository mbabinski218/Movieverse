using System.Net.Http.Headers;
using System.Text;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Movieverse.Application.Common.Settings;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Payments.PayPal.Requests;
using Movieverse.Contracts.Payments.PayPal.Responses;
using Movieverse.Domain.Common.Result;
using Newtonsoft.Json;

namespace Movieverse.Application.Payments.PayPal.Handlers;

public sealed class AuthorizationHandler : IRequestHandler<AuthorizationRequest, Result<AuthorizationResponse>>
{
	private readonly ILogger<AuthorizationHandler> _logger;
	private readonly IHttpClientFactory _httpClientFactory;
	private readonly PaymentsSettings _paymentsSettings;

	public AuthorizationHandler(ILogger<AuthorizationHandler> logger, IHttpClientFactory httpClientFactory, 
		IOptions<PaymentsSettings> paymentsSettings)
	{
		_logger = logger;
		_httpClientFactory = httpClientFactory;
		_paymentsSettings = paymentsSettings.Value;
	}

	public async Task<Result<AuthorizationResponse>> Handle(AuthorizationRequest request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("PayPal - Authorization starting...");

		try
		{
			var httpClient = _httpClientFactory.CreateClient("PayPalClient");

			var authorizationByteArray = Encoding.ASCII.GetBytes($"{_paymentsSettings.PayPal.ClientId}:{_paymentsSettings.PayPal.ClientSecret}");
			var authorizationString = Convert.ToBase64String(authorizationByteArray);
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authorizationString);

			var nameValueCollection = new List<KeyValuePair<string, string>>
			{
				new("grant_type", "client_credentials")
			};
			
			var response = await httpClient.PostAsync($"{_paymentsSettings.PayPal.BaseUrl}{PayPalEndpoints.authorization}",
				new FormUrlEncodedContent(nameValueCollection), cancellationToken);

			var responseString = await response.Content.ReadAsStringAsync(cancellationToken);
			var responseJson = JsonConvert.DeserializeObject<AuthorizationResponse>(responseString);

			if (responseJson is null)
			{
				_logger.LogDebug("Transaction failed. Response is null");
				return Error.Invalid(PaymentsResources.FailedToPay);
			}

			_logger.LogDebug("Transaction with app id: {id} completed", responseJson.app_id);
			return responseJson;
		}
		catch (Exception e)
		{
			_logger.LogDebug("Transaction failed: {msg}", e.Message);
			return Error.Invalid(PaymentsResources.FailedToPay);
		}
	}
}