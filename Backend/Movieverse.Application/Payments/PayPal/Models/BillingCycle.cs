// ReSharper disable InconsistentNaming
namespace Movieverse.Application.Payments.PayPal.Models;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
public sealed class BillingCycle
{
	public Frequency frequency { get; set; }
	public string tenure_type { get; set; }
	public int sequence { get; set; }
	public int total_cycles { get; set; }
	public PricingScheme pricing_scheme { get; set; }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
