using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Contracts.DataTransferObjects.Genre;
using Movieverse.Contracts.Queries.Media;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.QueryHandlers.Media;

public sealed class GetAllGenreHandler : IRequestHandler<GetAllGenresQuery, Result<IEnumerable<GenreDto>>>
{
	private readonly ILogger<GetAllGenreHandler> _logger;
	private readonly IMediaReadOnlyRepository _mediaRepository;

	public GetAllGenreHandler(ILogger<GetAllGenreHandler> logger, IMediaReadOnlyRepository mediaRepository)
	{
		_logger = logger;
		_mediaRepository = mediaRepository;
	}

	public async Task<Result<IEnumerable<GenreDto>>> Handle(GetAllGenresQuery request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Retrieving all genres");
		
		var genre = await _mediaRepository.GetAllGenresAsync(cancellationToken);
		return genre.IsSuccessful ? genre : genre.Error;
	}
}