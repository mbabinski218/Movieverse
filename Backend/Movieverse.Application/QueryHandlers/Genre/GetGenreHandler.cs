using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Contracts.DataTransferObjects.Genre;
using Movieverse.Contracts.Queries;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.QueryHandlers.Genre;

public sealed class GetGenreHandler : IRequestHandler<GetGenreQuery, Result<GenreDto>>
{
	private readonly ILogger<GetGenreHandler> _logger;
	private readonly IGenreRepository _genreRepository;
	private readonly IMapper _mapper;

	public GetGenreHandler(ILogger<GetGenreHandler> logger, IGenreRepository genreRepository, IMapper mapper)
	{
		_logger = logger;
		_genreRepository = genreRepository;
		_mapper = mapper;
	}

	public async Task<Result<GenreDto>> Handle(GetGenreQuery request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Retrieving genre with id: {Id}", request.Id);
		
		var genre = await _genreRepository.FindByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
		return genre.IsSuccessful ? _mapper.Map<GenreDto>(genre.Value) : genre.Error;
	}
}