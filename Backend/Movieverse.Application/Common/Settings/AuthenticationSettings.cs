using Movieverse.Application.Interfaces;

namespace Movieverse.Application.Common.Settings;

public sealed class AuthenticationSettings : ISettings
{
	public string Key => "Authentication";
	public Token Token { get; set; } = null!;
	public Google Google { get; set; } = null!;
	public Facebook Facebook { get; set; } = null!;
}

public sealed class Token
{
	public string Secret { get; set; } = null!;
	public string Issuer { get; set; } = null!;
	public string Audience { get; set; } = null!;
	public string TokenExpirationTime { get; set; } = null!;
}

public sealed class Google
{
	public string ClientId { get; set; } = null!;
	public string ClientSecret { get; set; } = null!;
}

public sealed class Facebook
{
	public string AppId { get; set; } = null!;
	public string AppSecret { get; set; } = null!;
}