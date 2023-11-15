// ReSharper disable InconsistentNaming
namespace Movieverse.Application.Payments.PayPal.Models;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
public sealed class Authorization
{
	public string scope { get; set; }
	public string access_token { get; set; }
	public string token_type { get; set; }
	public string app_id { get; set; }
	public int expires_in { get; set; }
	public List<string> supported_authn_schemes { get; set; }
	public string nonce { get; set; }
	public ClientMetadata client_metadata { get; set; }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.