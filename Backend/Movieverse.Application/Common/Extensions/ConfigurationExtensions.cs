using Microsoft.Extensions.Configuration;
using Movieverse.Application.Interfaces;

namespace Movieverse.Application.Common.Extensions;

public static class ConfigurationExtensions
{
	public static T Map<T>(this IConfiguration configuration) 
		where T : ISettings, new()
	{
		var settings = new T();
		configuration.Bind(settings.Key, settings);
		return settings;
	}
}