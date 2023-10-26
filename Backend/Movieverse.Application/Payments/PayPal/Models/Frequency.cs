namespace Movieverse.Application.Payments.PayPal.Models;

public sealed class Frequency
{
	public string interval_unit { get; set; }
	public int interval_count { get; set; }
}