using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Contracts.DataTransferObjects.Genre;
using Movieverse.Contracts.Queries.Genre;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.QueryHandlers.Genre;

public sealed class GetGenreHandler : IRequestHandler<GetGenreQuery, Result<GenreDto>>
{
	private readonly ILogger<GetGenreHandler> _logger;
	private readonly IGenreReadOnlyRepository _genreRepository;

	public GetGenreHandler(ILogger<GetGenreHandler> logger, IGenreReadOnlyRepository genreRepository)
	{
		_logger = logger;
		_genreRepository = genreRepository;
	}

	public async Task<Result<GenreDto>> Handle(GetGenreQuery request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Retrieving genre with id: {Id}", request.Id);
		
		var genre = await _genreRepository.FindAsync(request.Id, cancellationToken);
		return genre.IsSuccessful ? genre.Value : genre.Error;
	}
}