using Movieverse.Application.Interfaces;

namespace Movieverse.Application.Common.Settings;

public sealed class StatisticsSettings : ISettings
{
	public string Key => "Statistics";
	public string UpdateInterval { get; init; } = null!;
}