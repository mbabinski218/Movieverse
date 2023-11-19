using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Interfaces.Repositories;
using Movieverse.Application.Resources;
using Movieverse.Contracts.Commands.Media;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Common.Result;
using Movieverse.Domain.DomainEvents;
using Movieverse.Domain.Entities;
using Movieverse.Domain.ValueObjects;
using Movieverse.Domain.ValueObjects.Ids.AggregateRootIds;

namespace Movieverse.Application.CommandHandlers.MediaCommands.Update;

public sealed class UpdateMediaHandler : IRequestHandler<UpdateMediaCommand, Result<MediaDto>>
{
	private readonly ILogger<UpdateMediaHandler> _logger;
	private readonly IMediaRepository _mediaRepository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IOutputCacheStore _outputCacheStore;
	private readonly IMapper _mapper;
	
	public UpdateMediaHandler(ILogger<UpdateMediaHandler> logger, IMediaRepository mediaRepository, IUnitOfWork unitOfWork, 
		IOutputCacheStore outputCacheStore, IMapper mapper)
	{
		_logger = logger;
		_mediaRepository = mediaRepository;
		_unitOfWork = unitOfWork;
		_outputCacheStore = outputCacheStore;
		_mapper = mapper;
	}

	public async Task<Result<MediaDto>> Handle(UpdateMediaCommand request, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Updating media with id {Id}", request.Id);
		
		// Search for media
		var findResult = await _mediaRepository.FindFullAsync(request.Id, cancellationToken);
		if (!findResult.IsSuccessful)
		{
			return findResult.Error;
		}
		var media = findResult.Value;

		// Updating media
		if (request.Title is not null) media.Title = request.Title;
		
		media.Details = request.Details ?? new();
		media.Details.ReleaseDate = media.Details.ReleaseDate?.UtcDateTime;
		media.TechnicalSpecs = request.TechnicalSpecs ?? new();
		
		// Updating poster
		if (request.ChangePoster ?? false)
		{
			if (request.Poster is not null)
			{
				var posterId = media.PosterId ?? ContentId.Create();
				media.PosterId = posterId;
				media.AddDomainEvent(new ImageChanged(posterId, request.Poster));
			}
			else
			{
				media.PosterId = null;
			}
		}
		
		// Updating trailer
		if (request.ChangeTrailer ?? false)
		{
			if (request.Trailer is not null)
			{
				var trailerId = media.TrailerId ?? ContentId.Create();
				media.TrailerId = trailerId;
				media.AddDomainEvent(new VideoChanged(trailerId, request.Trailer));
			}
			else
			{
				media.TrailerId = null;
			}
		}
		
		// Updating images
		if (request.ImagesToAdd is not null)
		{
			foreach (var image in request.ImagesToAdd)
			{
				var imageId = ContentId.Create();
				media.AddContent(imageId);
				media.AddDomainEvent(new ImageChanged(imageId, image));
			}
		}
		
		// Updating videos
		if (request.VideosToAdd is not null)
		{
			foreach (var video in request.VideosToAdd)
			{
				var videoId = ContentId.Create();
				media.AddContent(videoId);
				media.AddDomainEvent(new VideoChanged(videoId, video));
			}
		}
		
		// Removing content
		if (request.ContentToRemove is not null)
		{
			foreach (var content in request.ContentToRemove)
			{
				media.RemoveContent(content);
			}
		}
		
		// Updating platforms
		media.ClearPlatforms();
		if (request.PlatformIds is not null)
		{
			foreach (var platformId in request.PlatformIds)
			{
				if (media.PlatformIds.Contains(PlatformId.Create(platformId)))
				{
					continue;
				}
				
				media.AddPlatform(platformId);
			}
		}
		
		// Updating genres
		foreach (var genre in media.Genres)
		{
			genre.RemoveMedia(media);
		}
		media.ClearGenres();
		if (request.GenreIds is not null)
		{
			foreach (var genreId in request.GenreIds)
			{
				var genreResult = await _mediaRepository.FindGenreAsync(genreId, cancellationToken);
				if (genreResult.IsUnsuccessful)
				{
					continue;
				}
				var genre = genreResult.Value;
				
				media.AddGenre(genre);
				genre.AddMedia(media);
			}
		}

		// Updating staff
		if (request.Staff is not null)
		{
			var currentStaff = media.Staff.ToList();
			foreach (var staff in currentStaff)
			{
				if (request.Staff.All(x => x.PersonId != staff.PersonId))
				{
					media.RemoveStaff(staff);
					media.AddDomainEvent(new PersonFromMediaRemoved(media.Id.Value, staff.PersonId));
				}
			}
			
			foreach (var staffDto in request.Staff)
			{
				var editStaff = media.Staff.FirstOrDefault(x => x.PersonId == staffDto.PersonId);
				if (editStaff is not null)
				{
					editStaff.Role = staffDto.Role;
					continue;
				}
				
				var staff = Staff.Create(media, staffDto.PersonId, staffDto.Role);
				media.AddStaff(staff);
				media.AddDomainEvent(new PersonToMediaAdded(media.Id.Value, staff.PersonId));
			}
		}
		else
		{
			foreach (var staff in media.Staff)
			{
				media.AddDomainEvent(new PersonFromMediaRemoved(media.Id.Value, staff.PersonId));
			}
			media.ClearStaff();
		}

		// Updating movie info
		if (request.MovieInfo is not null)
		{
			if (media is Movie movie)
			{
				if (request.MovieInfo.SequelId is not null)
				{
					var sequel = await _mediaRepository.FindAsync(request.MovieInfo.SequelId, cancellationToken);
					if (sequel.IsSuccessful)
					{
						movie.SequelId = MediaId.Create(sequel.Value.Id);
						movie.SequelTitle = sequel.Value.Title;
					}
				}
				if (request.MovieInfo.PrequelId is not null)
				{
					var prequel = await _mediaRepository.FindAsync(request.MovieInfo.PrequelId, cancellationToken);
					if (prequel.IsSuccessful)
					{
						movie.PrequelId = MediaId.Create(prequel.Value.Id);
						movie.PrequelTitle = prequel.Value.Title;
					}
				}
			}
			else
			{
				return Error.Invalid(MediaResources.MediaIsNotMovie);
			}
		}
		
		// Updating series info
		if (request.SeriesInfo is not null)
		{
			if (media is Series series)
			{
				series.ClearSeasons();
				
				series.SeriesEnded = request.SeriesInfo.SeriesEnded?.UtcDateTime;
				
				if (request.SeriesInfo.Seasons is not null)
				{
					foreach (var seasonDto in request.SeriesInfo.Seasons)
					{
						var season = series.Seasons.FirstOrDefault(x => x.SeasonNumber == seasonDto.SeasonNumber);
						if (season is null)
						{
							season = Season.Create(series, seasonDto.SeasonNumber, seasonDto.Episodes?.Count() ?? 0);
							series.AddSeason(season);
						}

						if (seasonDto.Episodes is null)
						{
							continue;
						}
						
						foreach (var episodeDto in seasonDto.Episodes)
						{
							var episode = season.Episodes.FirstOrDefault(x => x.EpisodeNumber == episodeDto.EpisodeNumber);
							if (episode is null)
							{
								var details = episodeDto.Details ?? new Details();
								details.ReleaseDate = details.ReleaseDate?.UtcDateTime;
								
								episode = Episode.Create(season, episodeDto.EpisodeNumber, episodeDto.Title ?? "", details);
								season.AddEpisode(episode);
							}
						}
					}
				}
			}
			else
			{
				return Error.Invalid(MediaResources.MediaIsNotSeries);
			}
		}
		
		// Database operations
		var updateResult = await _mediaRepository.UpdateAsync(media, cancellationToken);
		if (updateResult.IsUnsuccessful)
		{
			return updateResult.Error;
		}

		if (!await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false))
		{
			_logger.LogError("Media with id {Id} could not be updated.", request.Id);
			return Error.Invalid(MediaResources.CouldNotUpdateMedia);
		}
		
		// Evicting cache
		await _outputCacheStore.EvictByTagAsync(request.Id.ToString(), cancellationToken);
		return _mapper.Map<MediaDto>(media);
	}
}