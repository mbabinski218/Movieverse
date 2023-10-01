namespace Movieverse.Consumer.Common.Settings;

public sealed class DefaultSettings
{
	public const string key = "Defaults";
	public string HealthCheckEndpoint { get; init; } = null!;
}