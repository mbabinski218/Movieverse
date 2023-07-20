using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Movieverse.Application.Interfaces;

namespace Movieverse.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services)
	{
		AddRepositories(services);
		
		return services;
	}

	private static IServiceCollection AddRepositories(IServiceCollection services)
	{
		
		
		return services;
	}
}