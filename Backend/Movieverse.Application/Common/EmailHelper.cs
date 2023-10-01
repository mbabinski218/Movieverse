using System.Web;

namespace Movieverse.Application.Common;

public static class EmailHelper
{
	public static string CreateConfirmationLink(string? url, Guid userId, string token)
	{
		var id = userId.ToString();
		var encodedToken = HttpUtility.UrlEncode(token);
		return $"{url}/api/user/confirm-email?Id={id}&Token={encodedToken}";
	}
}