namespace Movieverse.Application.Payments.PayPal.Models;

public sealed class FixedPrice
{
	public string value { get; set; }
	public string currency_code { get; set; }
}