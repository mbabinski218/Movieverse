using System.Reflection;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using Movieverse.Application.Behaviors;

namespace Movieverse.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
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