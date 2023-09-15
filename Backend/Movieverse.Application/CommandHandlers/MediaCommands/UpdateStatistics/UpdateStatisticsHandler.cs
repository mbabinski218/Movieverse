using System.Collections.Concurrent;
using MediatR;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
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
		
		var medias = await _mediaRepository.GetAllAsync(cancellationToken).ConfigureAwait(false);
		if (medias.IsUnsuccessful)
		{
			return medias.Error;
		}
		
		_metrics = new ConcurrentDictionary<Guid, long>(_metricsService.GetCounters(Meter.mediaCounter));
		
		var maxPosition = medias.Value.Count;
		var empty = _metrics.Values.Count == 0;
		var minViews = empty ? 0 : _metrics.Values.Min();
		var maxViews = empty ? 0 : _metrics.Values.Max();
		var minBasicStatistics = new BasicStatistics
		{
			Rating = medias.Value.Min(x => x.BasicStatistics.Rating),
			Votes = medias.Value.Min(x => x.BasicStatistics.Votes),
			UserReviews = medias.Value.Min(x => x.BasicStatistics.UserReviews),
			CriticReviews = medias.Value.Min(x => x.BasicStatistics.CriticReviews),
			InWatchlistCount = medias.Value.Min(x => x.BasicStatistics.InWatchlistCount)
		};
		var maxBasicStatistics = new BasicStatistics
		{
			Rating = medias.Value.Max(x => x.BasicStatistics.Rating),
			Votes = medias.Value.Max(x => x.BasicStatistics.Votes),
			UserReviews = medias.Value.Max(x => x.BasicStatistics.UserReviews),
			CriticReviews = medias.Value.Max(x => x.BasicStatistics.CriticReviews),
			InWatchlistCount = medias.Value.Max(x => x.BasicStatistics.InWatchlistCount)
		};
		
		var rangePosition = (float)maxPosition - 1;
		var rangeViews = (float)maxViews - minViews;
		var rangeBasicStatistics = maxBasicStatistics - minBasicStatistics;
		
		var size = medias.Value.Count;
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
			var media = medias.Value.Skip(toSkip).Take(packageForTask[i]).ToList();
			
			tasks.Add(Task.Run(() => CalculatePoints(media, minViews, minBasicStatistics, rangePosition, rangeViews, rangeBasicStatistics), cancellationToken));
		}
		Task.WaitAll(tasks.ToArray(), cancellationToken);

		var ranking = _newRanking
			.OrderByDescending(x => x.Value)
			.Select(x => x.Key)
			.ToList();
		
		for (var i = 0; i < ranking.Count; i++)
		{
			var media = medias.Value.FirstOrDefault(x => x.Id == ranking[i]);
			if (media is null)
			{
				continue;
			}

			var position = i + 1;
			media.CurrentPosition = position;
			media.AdvancedStatistics.Popularity.Last().Position = position;
			media.AdvancedStatistics.Popularity.Last().Change = media.AdvancedStatistics.Popularity[^2].Position - position;
			
			await _outputCacheStore.EvictByTagAsync(media.Id.ToString(), cancellationToken).ConfigureAwait(false);
		}

		var updateResult = await _mediaRepository.UpdateRangeAsync(medias.Value, cancellationToken).ConfigureAwait(false);
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
	
	private void CalculatePoints(List<Media> medias, long minViews, BasicStatistics minBasicStatistics, float rangePosition, float rangeViews, 
		BasicStatistics rangeBasicStatistics)
	{
		foreach (var media in medias)
		{
			var lastPopularity = media.AdvancedStatistics.Popularity.Last();

			var latestPopularity = Popularity.Create(media.AdvancedStatistics, DateTimeOffset.UtcNow);
			latestPopularity.Views = _metrics.TryGetValue(media.Id, out var views) ? views : 0;
			latestPopularity.BasicStatistics = media.BasicStatistics;
				
			var viewsChange = latestPopularity.Views - lastPopularity.Views;
			var basicStatisticsChange = latestPopularity.BasicStatistics - lastPopularity.BasicStatistics;
				
			var normalizedPosition = (lastPopularity.Position - 1) / rangePosition;
			var normalizedViews = (viewsChange - minViews) / rangeViews;
			var normalizedVotes = (basicStatisticsChange.Votes - minBasicStatistics.Votes) / (float)rangeBasicStatistics.Votes;
			var normalizedUserReviews = (basicStatisticsChange.UserReviews - minBasicStatistics.UserReviews) / (float)rangeBasicStatistics.UserReviews;
			var normalizedCriticReviews = (basicStatisticsChange.CriticReviews - minBasicStatistics.CriticReviews) / (float)rangeBasicStatistics.CriticReviews;
			var normalizedInWatchlistCount = (basicStatisticsChange.InWatchlistCount - minBasicStatistics.InWatchlistCount) / (float)rangeBasicStatistics.InWatchlistCount;
				
			var points = normalizedViews + normalizedVotes * 2 + normalizedUserReviews * 4 + normalizedCriticReviews * 5 + 
			             normalizedInWatchlistCount * 3 + normalizedPosition * 3;
				
			media.AdvancedStatistics.Popularity.Add(latestPopularity);
			_newRanking.TryAdd(media.Id, points);
		}
	}
}