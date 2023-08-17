using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Movieverse.Application.Caching;

public static class OutputCacheServiceCollectionExtensions
{
	public static IServiceCollection AddRedisOutputCache(this IServiceCollection services)
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));
		
		services.AddOutputCache();

		services.RemoveAll<IOutputCacheStore>();
		services.AddSingleton<IOutputCacheStore, OutputCacheStore>();
		
		return services;
	}
	
	public static IServiceCollection AddRedisOutputCache(this IServiceCollection services, Action<OutputCacheOptions> options)
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));
		ArgumentNullException.ThrowIfNull(options, nameof(options));
		
		services.AddOutputCache(options);

		services.RemoveAll<IOutputCacheStore>();
		services.AddSingleton<IOutputCacheStore, OutputCacheStore>();
		
		return services;
	}
}