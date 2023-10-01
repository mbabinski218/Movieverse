using MediatR;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Commands.Media;
using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Application.CommandHandlers.MediaCommands.Add;

public sealed class AddMediaHandler : IRequestHandler<AddMediaCommand, Result>
{
	private readonly ILogger<AddMediaHandler> _logger;
	private readonly IMediaRepository _mediaRepository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IOutputCacheStore _outputCacheStore;

	public AddMediaHandler(ILogger<AddMediaHandler> logger, IMediaRepository mediaRepository, IUnitOfWork unitOfWork, 
		IOutputCacheStore outputCacheStore)
	{
		_logger = logger;
		_mediaRepository = mediaRepository;
		_unitOfWork = unitOfWork;
		_outputCacheStore = outputCacheStore;
	}

	public async Task<Result> Handle(AddMediaCommand request, CancellationToken cancellationToken)
	{
		if (await _mediaRepository.TitleExistsAsync(request.Title, cancellationToken).ConfigureAwait(false))
		{
			_logger.LogError("Media with title {Title} already exists", request.Title);
			return Error.AlreadyExists(MediaResources.MediaWithTitleAlreadyExists);
		}

		Result addResult;
		var mediaId = MediaId.Create();
		switch (request.Type)
		{
			case nameof(Movie):
				var movie = Movie.Create(mediaId, request.Title);
				addResult = await _mediaRepository.AddMovieAsync(movie, cancellationToken).ConfigureAwait(false);
				break;
			case nameof(Series):
				var series = Series.Create(mediaId, request.Title);
				addResult = await _mediaRepository.AddSeriesAsync(series, cancellationToken).ConfigureAwait(false);
				break; 
			default:
				addResult = Error.Invalid(MediaResources.InvalidMediaType);
				break;
		}

		if (addResult.IsUnsuccessful)
		{
			return addResult.Error;
		}

		if (!await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false))
		{
			return Error.Invalid(MediaResources.CouldNotAddMedia);
		}
		
		await _outputCacheStore.EvictByTagAsync(mediaId.ToString(), cancellationToken).ConfigureAwait(false);
		return Result.Ok();
	}
}