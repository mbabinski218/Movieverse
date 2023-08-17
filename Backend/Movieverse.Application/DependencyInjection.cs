using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using Amazon;
using Amazon.Runtime;
using FluentValidation;
using MassTransit;
using MediatR;
using Movieverse.Application.Behaviors;
using Movieverse.Application.Caching;
using Movieverse.Application.Common.Extensions;
using Movieverse.Application.Common.Settings;
using Movieverse.Application.Services;
using StackExchange.Redis;

namespace Movieverse.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddMediators(configuration);
		services.AddCacheProvider(configuration);
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
	
	private static IServiceCollection AddCacheProvider(this IServiceCollection services, IConfiguration configuration)
	{
		var cacheSettings = configuration.Map<CacheSettings>();
		
		var redisOptions = new ConfigurationOptions
		{
			EndPoints = { {cacheSettings.Redis.Url, cacheSettings.Redis.Port} },
			User = cacheSettings.Redis.User,
			Password = cacheSettings.Redis.Password
		};
		
		services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisOptions));
		services.AddRedisOutputCache(options =>
		{
			if (TimeSpan.TryParse(cacheSettings.ExpirationTime, out var expirationTime))
			{
				options.DefaultExpirationTimeSpan = expirationTime;
			}
		});
		
		return services;
	}
	
	private static IServiceCollection AddMediators(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddMediatR();
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		var queueSettings = configuration.Map<QueuesSettings>();
		
		services.AddMassTransit(queueSettings);
		
		return services;
	}

	private static IServiceCollection AddMediatR(this IServiceCollection services)
	{
		services.AddMediatR(cfg =>
		{
			cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

			cfg.AddOpenRequestPreProcessor(typeof(LoggingBehavior<>));
			cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
		});

		return services;
	}
	
	private static IServiceCollection AddMassTransit(this IServiceCollection services, QueuesSettings settings)
	{
		var credentials = new BasicAWSCredentials(settings.AmazonSQS.AccessKey, settings.AmazonSQS.SecretKey);
		var regionEndpoint = RegionEndpoint.GetBySystemName(settings.AmazonSQS.Host);
		
		services.AddMassTransit(busCfg =>
		{
			busCfg.UsingAmazonSqs((_, sqsCfg) =>
			{
				sqsCfg.Host(settings.AmazonSQS.Host, hostCfg =>
				{
					hostCfg.Credentials(credentials);
				});
                
				sqsCfg.MessageTopology.SetEntityNameFormatter(new EntityNameFormatter(settings.AmazonSQS.TopicName));
			});
		});

		services.AddHealthChecks()
			.AddSnsTopicsAndSubscriptions(options =>
			{
				options.Credentials = credentials;
				options.RegionEndpoint = regionEndpoint;

				options.AddTopicAndSubscriptions(settings.AmazonSQS.TopicName);
			}, "aws-sns");
		
		return services;
	}
}