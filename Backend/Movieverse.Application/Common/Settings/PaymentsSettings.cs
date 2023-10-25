using Movieverse.Application.Interfaces;

namespace Movieverse.Application.Common.Settings;

public sealed class PayPal : ISettings
{
	public string Key => "PayPal";
	public string BaseUrl { get; init; } = null!;
	public string ClientId { get; init; } = null!;
	public string ClientSecret { get; init; } = null!;
}

public sealed class Subscription : ISettings
{
	public string Key => "Subscription";
	public string Plan { get; init; } = null!;
	public string Price { get; init; } = null!;
	public string Currency { get; init; } = null!;
}

public sealed class PaymentsSettings : ISettings
{
	public string Key => "Payments";
	public PayPal PayPal { get; init; } = null!;
	public Subscription Subscription { get; init; } = null!;
}