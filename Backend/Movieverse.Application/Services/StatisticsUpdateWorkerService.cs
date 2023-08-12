using System.Globalization;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Movieverse.Application.Common.Settings;

namespace Movieverse.Application.Services;

public sealed class StatisticsUpdateWorkerService : BackgroundService
{
	private readonly ILogger<StatisticsUpdateWorkerService> _logger;
	//private readonly IMediaRepository _mediaRepository;
	private readonly PeriodicTimer? _timer;

	public StatisticsUpdateWorkerService(ILogger<StatisticsUpdateWorkerService> logger, /*IMediaRepository? mediaRepository,*/ 
		IOptions<StatisticsSettings> settings)
	{
		_logger = logger;
		//_mediaRepository = mediaRepository;

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
			_logger.LogInformation("Updating statistics...");
			// var result = await _mediaRepository.UpdateStatistics();
			//
			// if (!result.IsSuccessful)
			// {
			// 	_logger.LogError("Statistics update failed: {error}", string.Join(",", result.Error.Messages));
			// }
		}
		
		_logger.LogInformation("Statistics update worker service stopped.");
	}
}