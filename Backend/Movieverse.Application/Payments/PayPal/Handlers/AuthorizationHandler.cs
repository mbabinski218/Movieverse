using System.Net.Http.Headers;
using System.Net.Http.Json;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Movieverse.Application.Common.Settings;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Payments.PayPal.Requests;
using Movieverse.Contracts.Payments.PayPal.Responses;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.Payments.PayPal.Handlers;

public sealed class AuthorizationHandler : IRequestHandler<AuthorizationRequest, Result<AuthorizationResponse>>
{
	private readonly ILogger<AuthorizationHandler> _logger;
	private readonly IHttpClientFactory _httpClientFactory;
	private readonly Common.Settings.PayPal _settings;

	public AuthorizationHandler(ILogger<AuthorizationHandler> logger, IHttpClientFactory httpClientFactory, 
		IOptions<PaymentsSettings> paymentsSettings)
	{
		_logger = logger;
		_httpClientFactory = httpClientFactory;
		_settings = paymentsSettings.Value.PayPal;
	}

	public async Task<Result<AuthorizationResponse>> Handle(AuthorizationRequest request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("PayPal - Authorization starting...");

		try
		{
			using var httpClient = _httpClientFactory.CreateClient("PayPalClient");

			// var authorizationByteArray = Encoding.ASCII.GetBytes($"{_settings.ClientId}:{_settings.ClientSecret}");
			// var authorizationString = Convert.ToBase64String(authorizationByteArray);
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _settings.BasicAuth);

			var nameValueCollection = new List<KeyValuePair<string, string>>
			{
				new("grant_type", "client_credentials")
			};
			
			var requestUri = $"{_settings.Url}{_settings.Endpoints.Authorization}";
			var response = await httpClient.PostAsync(requestUri, new FormUrlEncodedContent(nameValueCollection), cancellationToken);
			var responseJson = await response.Content.ReadFromJsonAsync<Models.Authorization>(cancellationToken: cancellationToken);

			if (responseJson is null)
			{
				_logger.LogDebug("Transaction failed. Response is null");
				return Error.Invalid(PaymentsResources.FailedToPay);
			}

			var authorization = new AuthorizationResponse
			{
				AccessToken = responseJson.access_token
			};
			
			_logger.LogDebug("Transaction with app id: {id} completed", responseJson.app_id);
			return authorization;
		}
		catch (Exception e)
		{
			_logger.LogDebug("Transaction failed: {msg}", e.Message);
			return Error.Invalid(PaymentsResources.FailedToPay);
		}
	}
}