using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using FluentValidation;
using MassTransit;
using MediatR;
using Movieverse.Application.Behaviors;
using Movieverse.Application.Common;
using Movieverse.Application.Common.Settings;
using Movieverse.Application.Services;

namespace Movieverse.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddMediators(configuration);
		services.AddSettings(configuration);
		services.AddServices();
		
		return services;
	}
	private static IServiceCollection AddServices(this IServiceCollection services)
	{
		services.AddHostedService<StatisticsUpdateWorkerService>()
			.Configure<HostOptions>(hostOptions => hostOptions.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore);

		return services;
	}

	private static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<StatisticsSettings>(configuration.GetSection(StatisticsSettings.key));

		return services;
	}
	
	private static IServiceCollection AddMediators(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddMediatR(cfg =>
		{
			cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
			
			cfg.AddOpenRequestPreProcessor(typeof(LoggingBehavior<>));
			cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
		});
        
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		var queueSettings = new QueuesSettings();
		configuration.Bind(QueuesSettings.key, queueSettings);
		
		services.AddMassTransit(busCfg =>
		{
			busCfg.UsingAmazonSqs((_, sqsCfg) =>
			{
				sqsCfg.Host(queueSettings.AmazonSQS.Host, hostCfg =>
				{
					hostCfg.SecretKey(queueSettings.AmazonSQS.SecretKey);
					hostCfg.AccessKey(queueSettings.AmazonSQS.AccessKey);
				});
                
				sqsCfg.MessageTopology.SetEntityNameFormatter(new EntityNameFormatter(queueSettings.AmazonSQS.TopicName));
			});
		});
		
		return services;
	}
}