using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Movieverse.Application.Behaviors;
using Movieverse.Application.Common.Settings;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Services;

namespace Movieverse.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddMediator();
		services.AddSettings(configuration);
		services.AddServices();
		
		return services;
	}
	private static IServiceCollection AddServices(this IServiceCollection services)
	{
		services.AddSingleton<IEmailServiceProvider, EmailServiceProvider>();
		
		services.AddHostedService<StatisticsUpdateWorkerService>()
			.Configure<HostOptions>(hostOptions => hostOptions.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore);

		return services;
	}

	private static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<EmailServiceSettings>(configuration.GetSection(EmailServiceSettings.key));
		services.Configure<StatisticsSettings>(configuration.GetSection(StatisticsSettings.key));

		return services;
	}
	
	private static IServiceCollection AddMediator(this IServiceCollection services)
	{
		services.AddMediatR(cfg =>
		{
			cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
			
			cfg.AddOpenRequestPreProcessor(typeof(LoggingBehavior<>));
			
			cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
		});
        
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		return services;
	}
}