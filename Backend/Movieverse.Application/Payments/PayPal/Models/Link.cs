// ReSharper disable InconsistentNaming
namespace Movieverse.Application.Payments.PayPal.Models;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
public sealed class Link
{
	public string href { get; set; }
	public string rel { get; set; }
	public string method { get; set; }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.