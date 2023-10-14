using MediatR;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Commands.Genre;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Application.CommandHandlers.GenreCommands;

public sealed class AddGenreHandler : IRequestHandler<AddGenreCommand, Result>
{
	private readonly ILogger<AddGenreHandler> _logger;
	private readonly IGenreRepository _genreRepository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IOutputCacheStore _outputCacheStore;

	public AddGenreHandler(ILogger<AddGenreHandler> logger, IGenreRepository genreRepository, IUnitOfWork unitOfWork, 
		IOutputCacheStore outputCacheStore)
	{
		_logger = logger;
		_genreRepository = genreRepository;
		_unitOfWork = unitOfWork;
		_outputCacheStore = outputCacheStore;
	}

	public async Task<Result> Handle(AddGenreCommand request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Adding genre {name}...", request.Name);
		
		var genre = Genre.Create(request.Name, request.Description);
		var addResult = await _genreRepository.AddAsync(genre, cancellationToken);
		if (addResult.IsUnsuccessful)
		{
			_logger.LogDebug("Genre {name} could not be added.", request.Name);
			return addResult.Error;
		}
		
		if (!await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false))
		{
			_logger.LogDebug("Genre {name} could not be added.", request.Name);
			return Error.Invalid(GenreResources.CannotCreateGenre);
		}
        
		await _outputCacheStore.EvictByTagAsync(genre.Id.ToString(), cancellationToken);
		
		_logger.LogDebug("Genre {name} added.", request.Name);
		return Result.Ok();
	}
}