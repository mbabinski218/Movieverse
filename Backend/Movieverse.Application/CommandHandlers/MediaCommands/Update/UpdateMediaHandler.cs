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
		var findResult = await _mediaRepository.FindAsync(request.Id, cancellationToken);
		if (!findResult.IsSuccessful)
		{
			return findResult.Error;
		}
		var media = findResult.Value;

		// Update media
		if (request.Title is not null) media.Title = request.Title;
		
		if (request.Details is not null)
		{
			if (request.Details.Certificate is not null) media.Details.Certificate = request.Details.Certificate; 
			if (request.Details.Language is not null) media.Details.Language = request.Details.Language; 
			if (request.Details.Runtime is not null) media.Details.Runtime = request.Details.Runtime; 
			if (request.Details.Storyline is not null) media.Details.Storyline = request.Details.Storyline; 
			if (request.Details.Tagline is not null) media.Details.Tagline = request.Details.Tagline; 
			if (request.Details.FilmingLocations is not null) media.Details.FilmingLocations = request.Details.FilmingLocations; 
			if (request.Details.ReleaseDate is not null) media.Details.ReleaseDate = request.Details.ReleaseDate; 
			if (request.Details.CountryOfOrigin is not null) media.Details.CountryOfOrigin = request.Details.CountryOfOrigin; 
		}

		if (request.TechnicalSpecs is not null)
		{
			if (request.TechnicalSpecs.Camera is not null) media.TechnicalSpecs.Camera = request.TechnicalSpecs.Camera;
			if (request.TechnicalSpecs.Color is not null) media.TechnicalSpecs.Color = request.TechnicalSpecs.Color;
			if (request.TechnicalSpecs.AspectRatio is not null) media.TechnicalSpecs.AspectRatio = request.TechnicalSpecs.AspectRatio;
			if (request.TechnicalSpecs.NegativeFormat is not null) media.TechnicalSpecs.NegativeFormat = request.TechnicalSpecs.NegativeFormat;
			if (request.TechnicalSpecs.SoundMix is not null) media.TechnicalSpecs.SoundMix = request.TechnicalSpecs.SoundMix;
		}
		
		if (request.Poster is not null)
		{
			var posterId = media.PosterId ?? ContentId.Create();
			media.PosterId = posterId;
			media.AddDomainEvent(new ImageChanged(posterId, request.Poster));
		}
		
		if (request.Trailer is not null)
		{
			var trailerId = media.TrailerId ?? ContentId.Create();
			media.TrailerId = trailerId;
			media.AddDomainEvent(new VideoChanged(trailerId, request.Trailer));
		}
		
		if (request.ImagesToAdd is not null)
		{
			foreach (var image in request.ImagesToAdd)
			{
				var imageId = ContentId.Create();
				media.AddContent(imageId);
				media.AddDomainEvent(new ImageChanged(imageId, image));
			}
		}
		
		if (request.VideosToAdd is not null)
		{
			foreach (var video in request.VideosToAdd)
			{
				var videoId = ContentId.Create();
				media.AddContent(videoId);
				media.AddDomainEvent(new VideoChanged(videoId, video));
			}
		}
		
		if (request.ContentToRemove is not null)
		{
			foreach (var content in request.ContentToRemove)
			{
				media.RemoveContent(content);
			}
		}
		
		if (request.PlatformIds is not null)
		{
			foreach (var platformId in request.PlatformIds)
			{
				if (media.PlatformIds.Contains(PlatformId.Create(platformId)))
				{
					continue;
				}
				
				media.AddPlatform(platformId);
				media.AddDomainEvent(new PlatformToMediaAdded(media.Id.Value, platformId));
			}
		}
		
		if (request.GenreIds is not null)
		{
			foreach (var genreId in request.GenreIds)
			{
				if (media.GenreIds.Contains(GenreId.Create(genreId)))
				{
					continue;
				}
				
				media.AddGenre(genreId);
				media.AddDomainEvent(new GenreToMediaAdded(media.Id.Value, genreId));
			}
		}

		if (request.Staff is not null)
		{
			foreach (var staffDto in request.Staff)
			{
				if (media.Staff.Any(x => x.PersonId == staffDto.PersonId))
				{
					continue;
				}
				
				var staff = Staff.Create(media, staffDto.PersonId, staffDto.Role);
				media.AddStaff(staff);
				media.AddDomainEvent(new PersonToMediaAdded(media.Id.Value, staff.PersonId));
			}
		}

		if (request.MovieInfo is not null)
		{
			if (media is Movie movie)
			{
				if (request.MovieInfo.SequelId is not null) movie.SequelId = request.MovieInfo.SequelId;
				if (request.MovieInfo.PrequelId is not null) movie.PrequelId = request.MovieInfo.PrequelId;
			}
			else
			{
				return Error.Invalid(MediaResources.MediaIsNotMovie);
			}
		}
		
		if (request.SeriesInfo is not null)
		{
			if (media is Series series)
			{
				if (request.SeriesInfo.SeriesEnded is not null) series.SeriesEnded = request.SeriesInfo.SeriesEnded;
				
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
								episode = Episode.Create(season, episodeDto.EpisodeNumber, episodeDto.Title ?? "", episodeDto.Details ?? new Details());
								season.AddEpisode(episode);
							}
								
							if (episodeDto.ImagesToAdd is not null)
							{
								foreach (var image in episodeDto.ImagesToAdd)
								{
									var imageId = ContentId.Create();
									episode.AddContent(imageId);
									episode.AddDomainEvent(new ImageChanged(imageId, image));
								}
							}
		
							if (episodeDto.VideosToAdd is not null)
							{
								foreach (var video in episodeDto.VideosToAdd)
								{
									var videoId = ContentId.Create();
									episode.AddContent(videoId);
									episode.AddDomainEvent(new VideoChanged(videoId, video));
								}
							}
		
							if (episodeDto.ContentToRemove is not null)
							{
								foreach (var content in episodeDto.ContentToRemove)
								{
									episode.RemoveContent(content);
								}
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
		
		await _outputCacheStore.EvictByTagAsync(request.Id.ToString(), cancellationToken);
		return _mapper.Map<MediaDto>(media);
	}
}