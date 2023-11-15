// ReSharper disable InconsistentNaming
namespace Movieverse.Application.Payments.PayPal.Models;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
public sealed class ClientMetadata
{
	public string name { get; set; }
	public string display_name { get; set; }
	public string logo_uri { get; set; }
	public List<string> scopes { get; set; }
	public string ui_type { get; set; }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.