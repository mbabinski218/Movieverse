using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Movieverse.Application.Commands.MediaCommands.UpdateStatistics;
using Movieverse.Application.Common.Settings;

namespace Movieverse.Application.Services;

public sealed class StatisticsUpdateWorkerService : BackgroundService
{
	private readonly ILogger<StatisticsUpdateWorkerService> _logger;
	private readonly IServiceProvider _serviceProvider;
	private readonly PeriodicTimer? _timer;

	public StatisticsUpdateWorkerService(ILogger<StatisticsUpdateWorkerService> logger, IServiceProvider serviceProvider, 
		IOptions<StatisticsSettings> settings)
	{
		_logger = logger;
		_serviceProvider = serviceProvider;

		if (TimeSpan.TryParse(settings.Value.UpdateInterval, out var interval))
		{
			_timer = new PeriodicTimer(interval);
		}
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		if (_timer is null)
		{
			_logger.LogError("Invalid statistics update interval. Worker service will not start");
			return;
		}
		_logger.LogInformation("Statistics update worker service started.");

		while (await _timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
		{
			using var scope = _serviceProvider.CreateScope();
			var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
			await mediator.Send(new UpdateStatisticsCommand(), stoppingToken);
		}
		
		_logger.LogInformation("Statistics update worker service stopped.");
	}
}