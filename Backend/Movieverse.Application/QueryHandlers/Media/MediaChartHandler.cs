using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Contracts.Queries.Media;
using Movieverse.Domain.Common;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.QueryHandlers.Media;

public sealed class MediaChartHandler : IRequestHandler<MediaChartQuery, Result<IPaginatedList<SearchMediaDto>>>
{
	private readonly ILogger<MediaChartHandler> _logger;
	private readonly IMediaReadOnlyRepository _mediaRepository;

	public MediaChartHandler(ILogger<MediaChartHandler> logger, IMediaReadOnlyRepository mediaRepository)
	{
		_logger = logger;
		_mediaRepository = mediaRepository;
	}

	public async Task<Result<IPaginatedList<SearchMediaDto>>> Handle(MediaChartQuery request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Getting chart...");

		return request.Type switch
		{
			"movies" => request.Category switch
			{
				"top100" => await _mediaRepository.FindTop100MoviesAsync(request.PageNumber, request.PageSize, cancellationToken),
				"mostPopular" => await _mediaRepository.FindMostPopularMoviesAsync(request.PageNumber, request.PageSize, cancellationToken),
				"releaseCalendar" => await _mediaRepository.FindUpcomingMoviesAsync(request.PageNumber, request.PageSize, cancellationToken),
				_ => Error.Invalid(MediaResources.InvalidChartCategory)
			},
			"series" => request.Category switch
			{
				"top100" => await _mediaRepository.FindTop100SeriesAsync(request.PageNumber, request.PageSize, cancellationToken),
				"mostPopular" => await _mediaRepository.FindMostPopularSeriesAsync(request.PageNumber, request.PageSize, cancellationToken),
				"releaseCalendar" => await _mediaRepository.FindUpcomingSeriesAsync(request.PageNumber, request.PageSize, cancellationToken),
				_ => Error.Invalid(MediaResources.InvalidChartCategory)
			},
			_ => Error.Invalid(MediaResources.InvalidChartType)
		};
	}
}