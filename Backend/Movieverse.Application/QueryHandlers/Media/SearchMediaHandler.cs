using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Contracts.Queries.Media;
using Movieverse.Domain.Common;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.QueryHandlers.Media;

public sealed class SearchMediaHandler : IRequestHandler<SearchMediaQuery, Result<IPaginatedList<SearchMediaDto>>>
{
	private readonly ILogger<SearchMediaHandler> _logger;
	private readonly IMediaReadOnlyRepository _mediaRepository;

	public SearchMediaHandler(ILogger<SearchMediaHandler> logger, IMediaReadOnlyRepository mediaRepository)
	{
		_logger = logger;
		_mediaRepository = mediaRepository;
	}

	public async Task<Result<IPaginatedList<SearchMediaDto>>> Handle(SearchMediaQuery request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("SearchMediaHandler.Handle - Searching media with string {Search}", request.Term);
		
		return await _mediaRepository.SearchMediaAsync(request.Term, request.PageNumber, request.PageSize, cancellationToken).ConfigureAwait(false);
	}
}