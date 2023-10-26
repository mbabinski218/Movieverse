namespace Movieverse.Application.Payments.PayPal.Models;

public sealed class Taxes
{
	public string percentage { get; set; }
	public bool inclusive { get; set; }
}