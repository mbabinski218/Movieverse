using System.Reflection;
using Amazon.Runtime;
using MassTransit;
using Movieverse.Consumer.Common;
using Movieverse.Consumer.Common.Settings;

namespace Movieverse.Consumer;

public static class DependencyInjection
{
	public static IServiceCollection AddMassTransit(this IServiceCollection services, IConfiguration configuration)
	{
		var sqsSettings = new AmazonSQSSettings();
		configuration.Bind(AmazonSQSSettings.key, sqsSettings);
		
		var credentials = new BasicAWSCredentials(sqsSettings.AccessKey, sqsSettings.SecretKey);
		
		services.AddMassTransit(busCfg =>
		{
			busCfg.SetKebabCaseEndpointNameFormatter();
			busCfg.SetInMemorySagaRepositoryProvider();

			var assembly = Assembly.GetExecutingAssembly();
			busCfg.AddConsumers(assembly);
			busCfg.AddSagaStateMachines(assembly);
			busCfg.AddSagas(assembly);
			busCfg.AddActivities(assembly);

			busCfg.UsingAmazonSqs((context, sqsCfg) =>
			{
				sqsCfg.Host(sqsSettings.Host, hostCfg =>
				{
					hostCfg.Credentials(credentials);
				});

				sqsCfg.ReceiveEndpoints(sqsSettings, context);
			});
		});
		
		return services;
	}
}