using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using FluentValidation;
using Mapster;
using MapsterMapper;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Movieverse.Application.Authorization;
using Movieverse.Application.Authorization.Handlers;
using Movieverse.Application.Authorization.Requirements;
using Movieverse.Application.Behaviors;
using Movieverse.Application.Caching.Extensions;
using Movieverse.Application.Caching.Policies;
using Movieverse.Application.Common.Extensions;
using Movieverse.Application.Common.Settings;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Metrics;
using Movieverse.Application.Services;
using Movieverse.Domain.Common.Types;
using StackExchange.Redis;

namespace Movieverse.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddSettings(configuration);
		services.AddMediators(configuration);
		services.AddCacheProvider(configuration);
		services.AddCloudStore(configuration);
		services.AddAuthorization(configuration);
		services.AddMapper();
		services.AddServices();
		services.AddMetrics();
		
		return services;
	}
	
	private static IServiceCollection AddServices(this IServiceCollection services)
	{
		services.AddSingleton<IImageService, ImageService>();
		
		services.AddHostedService<StatisticsUpdateWorkerService>()
			.Configure<HostOptions>(hostOptions => hostOptions.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore);

		return services;
	}

	private static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
	{
		services.BindSettings<StatisticsSettings>(configuration);
		services.BindSettings<CloudStoreSettings>(configuration);
		
		return services;
	}
	
	private static IServiceCollection AddCacheProvider(this IServiceCollection services, IConfiguration configuration)
	{
		var cacheSettings = configuration.Map<CacheSettings>();

		var connectionMultiplexer = ConnectionMultiplexer.Connect(
			new ConfigurationOptions
			{
				EndPoints = {{cacheSettings.Redis.Url, cacheSettings.Redis.Port}},
				Password = cacheSettings.Redis.Password
			});
		
		services.AddSingleton<IConnectionMultiplexer>(connectionMultiplexer);
		
		services.AddRedisOutputCache(options =>
		{
			options.AddBasePolicy(builder =>
			{
				builder.AddPolicy<DefaultOutputCachePolicy>();
				builder.AddPolicy<ByIdOutputCachePolicy>();
				builder.Expire(TimeSpan.Parse(cacheSettings.ExpirationTime));
			}, true);
		});

		services.AddHealthChecks()
			.AddRedis(connectionMultiplexer, "redis");
        
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
	
	private static IServiceCollection AddCloudStore(this IServiceCollection services, IConfiguration configuration)
	{
		var settings = configuration.Map<CloudStoreSettings>();
		
		var credentials = new BasicAWSCredentials(settings.AmazonS3.AccessKey, settings.AmazonS3.SecretKey);
		var regionEndpoint = RegionEndpoint.GetBySystemName(settings.AmazonS3.Host);
		
		var amazonS3Client = new AmazonS3Client(credentials, regionEndpoint);
		services.AddSingleton<IAmazonS3>(amazonS3Client);
		
		return services;
	}
	
	private static IServiceCollection AddAuthorization(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddScoped<IAuthorizationHandler, RoleHandler>();
		services.AddScoped<IAuthorizationHandler, OneOfRoleHandler>();
		services.AddScoped<IAuthorizationHandler, PersonalDataHandler>();
		
		services.AddAuthorization(options =>
		{
			options.AddPolicy(Policies.administrator, policy => 
				policy.Requirements.Add(new RoleRequirement(UserRole.Administrator)));
			
			options.AddPolicy(Policies.critic, policy =>
				policy.Requirements.Add(new RoleRequirement(UserRole.Critic)));
			
			options.AddPolicy(Policies.atLeastPro, policy =>
				policy.Requirements.Add(new OneOfRoleRequirement(UserRole.Pro, UserRole.Critic, UserRole.Administrator)));
			
			options.AddPolicy(Policies.atLeastUser, policy =>
				policy.Requirements.Add(new OneOfRoleRequirement(UserRole.User, UserRole.Pro, UserRole.Critic, UserRole.Administrator)));
			
			options.AddPolicy(Policies.personalData, policy =>
				policy.Requirements.Add(new PersonalDataRequirement()));
		});
		
		return services;
	}
	
	private static IServiceCollection AddMetrics(this IServiceCollection services)
	{
		services.AddSingleton<IMetricsService, MetricsService>();
		services.AddSingleton<MetricsMiddleware>();

		return services;
	}
}