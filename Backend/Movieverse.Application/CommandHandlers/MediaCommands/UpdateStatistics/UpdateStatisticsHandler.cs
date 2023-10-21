using System.Collections.Concurrent;
using MediatR;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Metrics;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Commands.Media;
using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.Entities;
using Movieverse.Domain.ValueObjects;

namespace Movieverse.Application.CommandHandlers.MediaCommands.UpdateStatistics;

public sealed class UpdateStatisticsHandler : IRequestHandler<UpdateStatisticsCommand, Result>
{
	private readonly ILogger<UpdateStatisticsHandler> _logger;
	private readonly IMediaRepository _mediaRepository;
	private readonly IMetricsService _metricsService;
	private readonly IOutputCacheStore _outputCacheStore;
	private readonly IUnitOfWork _unitOfWork;

	private ConcurrentDictionary<Guid, long> _metrics = new();
	private readonly ConcurrentDictionary<Guid, double> _newRanking = new();
	
	public UpdateStatisticsHandler(ILogger<UpdateStatisticsHandler> logger, IMediaRepository mediaRepository, IMetricsService metricsService, 
		IOutputCacheStore outputCacheStore, IUnitOfWork unitOfWork)
	{
		_logger = logger;
		_mediaRepository = mediaRepository;
		_metricsService = metricsService;
		_outputCacheStore = outputCacheStore;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result> Handle(UpdateStatisticsCommand request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Updating statistics...");
		
		var media = await _mediaRepository.GetAllAsync(cancellationToken);
		if (media.IsUnsuccessful)
		{
			return media.Error;
		}
		
		_metrics = new ConcurrentDictionary<Guid, long>(_metricsService.GetCounters(Meter.mediaCounter));
		
		var maxPosition = media.Value.Count;
		var empty = _metrics.Values.Count == 0;
		var minViews = empty ? 0 : _metrics.Values.Min();
		var maxViews = empty ? 0 : _metrics.Values.Max();
		var minBasicStatistics = new BasicStatistics
		{
			Rating = media.Value.Min(x => x.BasicStatistics.Rating),
			Votes = media.Value.Min(x => x.BasicStatistics.Votes),
			UserReviews = media.Value.Min(x => x.BasicStatistics.UserReviews),
			CriticReviews = media.Value.Min(x => x.BasicStatistics.CriticReviews),
			OnWatchlistCount = media.Value.Min(x => x.BasicStatistics.OnWatchlistCount)
		};
		var maxBasicStatistics = new BasicStatistics
		{
			Rating = media.Value.Max(x => x.BasicStatistics.Rating),
			Votes = media.Value.Max(x => x.BasicStatistics.Votes),
			UserReviews = media.Value.Max(x => x.BasicStatistics.UserReviews),
			CriticReviews = media.Value.Max(x => x.BasicStatistics.CriticReviews),
			OnWatchlistCount = media.Value.Max(x => x.BasicStatistics.OnWatchlistCount)
		};
		
		var rangePosition = (float)maxPosition - 1;
		var rangeViews = (float)maxViews - minViews;
		var rangeBasicStatistics = maxBasicStatistics - minBasicStatistics;
		
		var size = media.Value.Count;
		var numberOfThreads = Environment.ProcessorCount;
		var package = size / numberOfThreads;
		var rest = size % numberOfThreads;
		
		var packageForTask = Enumerable.Repeat(package, numberOfThreads).ToArray();
		for (var i = rest; i > 0; i--)
		{
			packageForTask[i]++;
		}
		
		var tasks = new List<Task>();
		for (var i = 0; i < numberOfThreads; i++)
		{
			var toSkip = packageForTask.Take(i).Sum();
			var mediaToCalculate = media.Value.Skip(toSkip).Take(packageForTask[i]).ToList();
			
			tasks.Add(Task.Run(() => CalculatePoints(mediaToCalculate, minViews, minBasicStatistics, rangePosition, rangeViews, rangeBasicStatistics), cancellationToken));
		}
		Task.WaitAll(tasks.ToArray(), cancellationToken);

		var ranking = _newRanking
			.OrderByDescending(x => x.Value)
			.Select(x => x.Key)
			.ToList();
		
		for (var i = 0; i < ranking.Count; i++)
		{
			var mediaToCalculate = media.Value.FirstOrDefault(x => x.Id == ranking[i]);
			if (mediaToCalculate is null)
			{
				continue;
			}

			var position = i + 1;
			mediaToCalculate.CurrentPosition = position;
			mediaToCalculate.AdvancedStatistics.Popularity[^1].Position = position;
			mediaToCalculate.AdvancedStatistics.Popularity[^1].Change = mediaToCalculate.AdvancedStatistics.Popularity[^2].Position - position;
			
			await _outputCacheStore.EvictByTagAsync(mediaToCalculate.Id.ToString(), cancellationToken);
		}

		var updateResult = await _mediaRepository.UpdateRangeAsync(media.Value, cancellationToken);
		if (updateResult.IsUnsuccessful)
		{
			return updateResult.Error;
		}
		
		if (!await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false))
		{
			_logger.LogError("Could not update statistics");
			return Error.Invalid(MediaResources.CouldNotUpdateMedia);
		}
		
		_logger.LogDebug("Statistics updated successfully");
		return Result.Ok();
	}
	
	private void CalculatePoints(List<Media> media, long minViews, BasicStatistics minBasicStatistics, float rangePosition, float rangeViews, 
		BasicStatistics rangeBasicStatistics)
	{
		foreach (var mediaToCalculate in media)
		{
			var lastPopularity = mediaToCalculate.AdvancedStatistics.Popularity.Last();

			var latestPopularity = Popularity.Create(mediaToCalculate.AdvancedStatistics, DateTimeOffset.UtcNow);
			latestPopularity.Views = _metrics.TryGetValue(mediaToCalculate.Id, out var views) ? views : 0;
			latestPopularity.BasicStatistics = mediaToCalculate.BasicStatistics;
				
			var viewsChange = latestPopularity.Views - lastPopularity.Views;
			var basicStatisticsChange = latestPopularity.BasicStatistics - lastPopularity.BasicStatistics;
				
			var normalizedPosition = (lastPopularity.Position - 1) / rangePosition;
			var normalizedViews = (viewsChange - minViews) / rangeViews;
			var normalizedVotes = (basicStatisticsChange.Votes - minBasicStatistics.Votes) / (float)rangeBasicStatistics.Votes;
			var normalizedUserReviews = (basicStatisticsChange.UserReviews - minBasicStatistics.UserReviews) / (float)rangeBasicStatistics.UserReviews;
			var normalizedCriticReviews = (basicStatisticsChange.CriticReviews - minBasicStatistics.CriticReviews) / (float)rangeBasicStatistics.CriticReviews;
			var normalizedOnWatchlistCount = (basicStatisticsChange.OnWatchlistCount - minBasicStatistics.OnWatchlistCount) / (float)rangeBasicStatistics.OnWatchlistCount;
				
			var points = normalizedViews + normalizedVotes * 2 + normalizedUserReviews * 4 + normalizedCriticReviews * 5 + 
			             normalizedOnWatchlistCount * 3 + normalizedPosition * 3;
				
			mediaToCalculate.AdvancedStatistics.AddPopularity(latestPopularity);
			_newRanking.TryAdd(mediaToCalculate.Id, points);
		}
	}
}