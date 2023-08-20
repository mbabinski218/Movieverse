using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using Amazon;
using Amazon.Runtime;
using FluentValidation;
using Mapster;
using MapsterMapper;
using MassTransit;
using MediatR;
using Movieverse.Application.Behaviors;
using Movieverse.Application.Caching.Extensions;
using Movieverse.Application.Caching.Policies;
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
		services.AddSettings(configuration);
		services.AddCacheProvider(configuration);
		services.AddServices();
		services.AddMapper();
		
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
		services.AddOptions<StatisticsSettings>(configuration);
		
		return services;
	}
	
	private static IServiceCollection AddCacheProvider(this IServiceCollection services, IConfiguration configuration)
	{
		var cacheSettings = configuration.Map<CacheSettings>();

		services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(
			new ConfigurationOptions
			{
				EndPoints = { { cacheSettings.Redis.Url, cacheSettings.Redis.Port } },
				Password = cacheSettings.Redis.Password
			})
		);
		services.AddRedisOutputCache(options =>
		{
			options.AddBasePolicy(builder =>
			{
				builder.AddPolicy<DefaultOutputCachePolicy>();
				builder.AddPolicy<ByIdOutputCachePolicy>();
				builder.Expire(TimeSpan.Parse(cacheSettings.ExpirationTime));
			}, true);
		});
        
		return services;
	}
	
	private static IServiceCollection AddMediators(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddMediatR();
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
		services.AddMassTransit(configuration);
		
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
	
	private static IServiceCollection AddMassTransit(this IServiceCollection services, IConfiguration configuration)
	{
		var queueSettings = configuration.Map<QueuesSettings>();
		
		var credentials = new BasicAWSCredentials(queueSettings.AmazonSQS.AccessKey, queueSettings.AmazonSQS.SecretKey);
		var regionEndpoint = RegionEndpoint.GetBySystemName(queueSettings.AmazonSQS.Host);
		
		services.AddMassTransit(busCfg =>
		{
			busCfg.UsingAmazonSqs((_, sqsCfg) =>
			{
				sqsCfg.Host(queueSettings.AmazonSQS.Host, hostCfg =>
				{
					hostCfg.Credentials(credentials);
				});
                
				sqsCfg.UseNewtonsoftJsonSerializer();
				
				sqsCfg.MessageTopology.SetEntityNameFormatter(new EntityNameFormatter(queueSettings.AmazonSQS.TopicName));
			});
		});

		services.AddHealthChecks()
			.AddSnsTopicsAndSubscriptions(options =>
			{
				options.Credentials = credentials;
				options.RegionEndpoint = regionEndpoint;

				options.AddTopicAndSubscriptions(queueSettings.AmazonSQS.TopicName);
			}, "aws-sns");
		
		return services;
	}
	
	private static IServiceCollection AddMapper(this IServiceCollection services)
	{
		var config = TypeAdapterConfig.GlobalSettings;
		config.Scan(Assembly.GetExecutingAssembly());
		
		services.AddSingleton(config);
		services.AddScoped<IMapper, Mapper>();
		
		return services;
	}
}