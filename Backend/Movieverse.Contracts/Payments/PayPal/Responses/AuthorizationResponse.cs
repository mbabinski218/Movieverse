namespace Movieverse.Contracts.Payments.PayPal.Responses;

public sealed class AuthorizationResponse
{
	public string AccessToken { get; init; } = null!;
}