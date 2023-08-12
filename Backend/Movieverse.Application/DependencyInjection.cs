using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
		services.AddMediatR(cfg =>
		{
			cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
			
			cfg.AddOpenRequestPreProcessor(typeof(LoggingBehavior<>));
			
			cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
		});
        
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		services.AddSingleton<IEmailServiceProvider, EmailServiceProvider>();
		
		services.AddHostedService<StatisticsUpdateWorkerService>();
		
		var emailServiceSettings = new EmailServiceSettings();
		configuration.Bind(EmailServiceSettings.sectionName, emailServiceSettings);
		services.AddSingleton(Options.Create(emailServiceSettings));
		
		var statisticsSettings = new StatisticsSettings();
		configuration.Bind(StatisticsSettings.sectionName, statisticsSettings);
		services.AddSingleton(Options.Create(statisticsSettings));
		
		return services;
	}
}