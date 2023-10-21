using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Contracts.Queries.Media;
using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Common;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.QueryHandlers.Media;

public sealed class SearchMediaWithFiltersHandler : IRequestHandler<SearchMediaWithFiltersQuery, Result<IPaginatedList<SearchMediaDto>>>
{
	private readonly ILogger<SearchMediaWithFiltersHandler> _logger;
	private readonly IMediaReadOnlyRepository _mediaRepository;

	public SearchMediaWithFiltersHandler(ILogger<SearchMediaWithFiltersHandler> logger, IMediaReadOnlyRepository mediaRepository)
	{
		_logger = logger;
		_mediaRepository = mediaRepository;
	}

	public async Task<Result<IPaginatedList<SearchMediaDto>>> Handle(SearchMediaWithFiltersQuery request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("SearchMediaHandler.Handle - Searching media with string {Search}", request.Term);
		
		return request.Type switch 
		{
			nameof(Movie) => await _mediaRepository.SearchMoviesWithFiltersAsync(request.Term, request.GenreId, request.PageNumber, request.PageSize, cancellationToken),
			nameof(Series) => await _mediaRepository.SearchSeriesWithFiltersAsync(request.Term, request.GenreId, request.PageNumber, request.PageSize, cancellationToken),
			_ => Error.Invalid(MediaResources.InvalidMediaType)
		};
	}
}