namespace Movieverse.Application.Common.Settings;

public sealed class StatisticsSettings
{
	public const string key = "Statistics";
	public string UpdateInterval { get; init; } = null!;
}