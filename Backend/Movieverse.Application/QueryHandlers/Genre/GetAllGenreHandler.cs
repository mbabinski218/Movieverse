using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Contracts.DataTransferObjects.Genre;
using Movieverse.Contracts.Queries.Genre;
using Movieverse.Domain.Common;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.QueryHandlers.Genre;

public sealed class GetAllGenreHandler : IRequestHandler<GetAllGenresQuery, Result<IPaginatedList<GenreDto>>>
{
	private readonly ILogger<GetAllGenreHandler> _logger;
	private readonly IGenreReadOnlyRepository _genreRepository;

	public GetAllGenreHandler(ILogger<GetAllGenreHandler> logger, IGenreReadOnlyRepository genreRepository)
	{
		_logger = logger;
		_genreRepository = genreRepository;
	}

	public async Task<Result<IPaginatedList<GenreDto>>> Handle(GetAllGenresQuery request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Retrieving all genres");
		
		var genre = await _genreRepository.GetAllAsync(request.PageNumber, request.PageSize, cancellationToken).ConfigureAwait(false);
		return genre.IsSuccessful ? genre : genre.Error;
	}
}