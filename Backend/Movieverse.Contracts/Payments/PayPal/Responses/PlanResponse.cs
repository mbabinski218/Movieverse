namespace Movieverse.Contracts.Payments.PayPal.Responses;

public sealed class PlanResponse
{
	public string Name { get; init; } = null!;
	public string Description { get; init; } = null!;
	public string Price { get; init; } = null!;
	public string Currency { get; init; } = null!;
}