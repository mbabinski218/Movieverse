namespace Movieverse.Contracts.Payments.PayPal.Responses;

public sealed class SubscriptionResponse
{
	public DateTimeOffset? NextBillingTime { get; set; }
}