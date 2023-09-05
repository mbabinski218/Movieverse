using MediatR;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Common.Exceptions;
using Movieverse.Application.Interfaces;
using Movieverse.Domain.DomainEvents;

namespace Movieverse.Application.EventHandlers;

public sealed class GenreToMediaAddedHandler : INotificationHandler<GenreToMediaAdded>
{
	private readonly ILogger<GenreToMediaAddedHandler> _logger;
	private readonly IGenreRepository _genreRepository;
	private readonly IOutputCacheStore _outputCacheStore;

	public GenreToMediaAddedHandler(ILogger<GenreToMediaAddedHandler> logger, IGenreRepository genreRepository,
		IOutputCacheStore outputCacheStore)
	{
		_logger = logger;
		_genreRepository = genreRepository;
		_outputCacheStore = outputCacheStore;
	}

	public async Task Handle(GenreToMediaAdded notification, CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Adding media with id {MediaId} to platform with id {PlatformId}", notification.MediaId.ToString(),
			notification.GenreId.ToString());

		var genre = await _genreRepository.FindByIdAsync(notification.GenreId, cancellationToken).ConfigureAwait(false);
		ResultException.ThrowIfUnsuccessful(genre);

		genre.Value.MediaIds.Add(notification.MediaId);
		await _outputCacheStore.EvictByTagAsync(notification.GenreId.ToString(), cancellationToken);
	}
}