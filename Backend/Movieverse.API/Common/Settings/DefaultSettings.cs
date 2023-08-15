namespace Movieverse.API.Common.Settings;

public sealed class DefaultSettings
{
	public const string key = "Defaults";
	public string DatabaseName { get; init; } = null!;
	public Routes Routes { get; init; } = null!;
}

public sealed class Routes
{
	public string Origin { get; init; } = null!;
	public string AllowedHosts { get; init; } = null!;
	public string HealthCheckEndpoint { get; init; } = null!;
}