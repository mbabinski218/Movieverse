// ReSharper disable InconsistentNaming
// ReSharper disable CollectionNeverUpdated.Global
namespace Movieverse.Application.Payments.PayPal.Models;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
public sealed class Plan
{
	public string id { get; set; }
	public string product_id { get; set; }
	public string name { get; set; }
	public string description { get; set; }
	public string status { get; set; }
	public List<BillingCycle> billing_cycles { get; set; }
	public PaymentPreferences payment_preferences { get; set; }
	public Taxes taxes { get; set; }
	public DateTime create_time { get; set; }
	public DateTime update_time { get; set; }
	public List<Link> links { get; set; }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.