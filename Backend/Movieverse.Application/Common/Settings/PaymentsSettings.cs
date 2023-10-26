using Movieverse.Application.Interfaces;

namespace Movieverse.Application.Common.Settings;

public sealed class Endpoints : ISettings
{
	public string Key => "Endpoints";
	public string Authorization { get; init; } = null!;
	public string Plans { get; init; } = null!;
	public string Subscriptions { get; init; } = null!;
} 

public sealed class PayPal : ISettings
{
	public string Key => "PayPal";
	public string ClientId { get; init; } = null!;
	public string ClientSecret { get; init; } = null!;
	public string BasicAuth { get; init; } = null!;
	public string Url { get; init; } = null!;
	public Endpoints Endpoints { get; init; } = null!;
	public string PlanId { get; init; } = null!;
}

public sealed class PaymentsSettings : ISettings
{
	public string Key => "Payments";
	public PayPal PayPal { get; init; } = null!;
}