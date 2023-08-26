using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Movieverse.Application.Interfaces;

namespace Movieverse.Application.Common.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection BindSettings<T>(this IServiceCollection services, IConfiguration configuration)
		where T : class, ISettings, new()
	{
		var settings = configuration.Map<T>();
		services.AddSingleton(Options.Create(settings));

		return services;
	}
}