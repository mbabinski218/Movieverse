using Movieverse.Application.Interfaces;

namespace Movieverse.Application.Common.Settings;

public sealed class CacheSettings : ISettings
{
	public string Key => "Cache";
	public Redis Redis { get; init; } = null!;
	public string ExpirationTime { get; init; } = null!;
}

public sealed class Redis
{
	public string Url { get; init; } = null!;
	public int Port { get; init; }
	public string User { get; init; } = null!;
	public string Password { get; init; } = null!;
}
