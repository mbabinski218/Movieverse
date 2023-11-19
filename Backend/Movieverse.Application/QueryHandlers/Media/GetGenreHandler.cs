using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Contracts.DataTransferObjects.Genre;
using Movieverse.Contracts.Queries.Media;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.QueryHandlers.Media;

public sealed class GetGenreHandler : IRequestHandler<GetGenreQuery, Result<IEnumerable<GenreDto>>>
{
	private readonly ILogger<GetGenreHandler> _logger;
	private readonly IMediaReadOnlyRepository _mediaRepository;

	public GetGenreHandler(ILogger<GetGenreHandler> logger, IMediaReadOnlyRepository mediaRepository)
	{
		_logger = logger;
		_mediaRepository = mediaRepository;
	}

	public async Task<Result<IEnumerable<GenreDto>>> Handle(GetGenreQuery request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Retrieving genre info for media with id {Id}", request.Id);
		
		var genres = await _mediaRepository.GetGenresAsync(request.Id, cancellationToken);
		return genres;
	}
}