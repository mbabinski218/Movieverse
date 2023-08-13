using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Movieverse.Application.Commands.MediaCommands.UpdateStatistics;
using Movieverse.Application.Common.Settings;

namespace Movieverse.Application.Services;

public sealed class StatisticsUpdateWorkerService : BackgroundService
{
	private readonly ILogger<StatisticsUpdateWorkerService> _logger;
	private readonly IMediator _mediator;
	private readonly PeriodicTimer? _timer;

	public StatisticsUpdateWorkerService(ILogger<StatisticsUpdateWorkerService> logger, IMediator mediator,
		IOptions<StatisticsSettings> settings)
	{
		_logger = logger;
		_mediator = mediator;

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
			await _mediator.Send(new UpdateStatisticsCommand(), stoppingToken);
		}
		
		_logger.LogInformation("Statistics update worker service stopped.");
	}
}