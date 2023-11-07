using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Contracts.DataTransferObjects.Genre;
using Movieverse.Contracts.Queries.Media;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.QueryHandlers.Media;

public sealed class GetGenreHandler : IRequestHandler<GetGenreQuery, Result<IEnumerable<GenreInfoDto>>>
{
	private readonly ILogger<GetGenreHandler> _logger;
	private readonly IMediaReadOnlyRepository _mediaRepository;
	private readonly IGenreReadOnlyRepository _genreRepository;

	public GetGenreHandler(ILogger<GetGenreHandler> logger, IMediaReadOnlyRepository mediaRepository, IGenreReadOnlyRepository genreRepository)
	{
		_logger = logger;
		_mediaRepository = mediaRepository;
		_genreRepository = genreRepository;
	}

	public async Task<Result<IEnumerable<GenreInfoDto>>> Handle(GetGenreQuery request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Retrieving genre info for media with id {Id}", request.Id);
		
		var genreIds = await _mediaRepository.GetGenreIdsAsync(request.Id, cancellationToken);
		if (genreIds.IsUnsuccessful)
		{
			return genreIds.Error;
		}

		return await _genreRepository.GetGenresInfoAsync(genreIds.Value, cancellationToken);
	}
}