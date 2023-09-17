using Mapster;
using Movieverse.Application.Common.Extensions;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Domain.AggregateRoots.Media;
using Movieverse.Domain.Entities;

namespace Movieverse.Application.Common.MapperConfigs;

public sealed class MediaMapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<Media, MediaInfoDto>()
			.Map(dest => dest.Title, src => src.Title)
			.Map(dest => dest.Certificate, src => src.Details.Certificate)
			.Map(dest => dest.Runtime, src => src.Details.Runtime)
			.Map(dest => dest.Storyline, src => src.Details.Storyline)
			.Map(dest => dest.Id, src => src.Id)
			.Map(dest => dest.PosterId, src => src.PosterId.GetValue())
			.Map(dest => dest.Rating, src => src.BasicStatistics.Rating)
			.Map(dest => dest.Type, src => src.GetType().Name)
			.Map(dest => dest.StartYear, src => GetStartYear(src.Details.ReleaseDate));
		
		config.NewConfig<Media, MediaShortInfoDto>()
			.Map(dest => dest.Title, src => src.Title)
			.Map(dest => dest.Certificate, src => src.Details.Certificate)
			.Map(dest => dest.Id, src => src.Id)
			.Map(dest => dest.PosterId, src => src.PosterId.GetValue())
			.Map(dest => dest.Rating, src => src.BasicStatistics.Rating)
			.Map(dest => dest.StartYear, src => GetStartYear(src.Details.ReleaseDate));

		config.NewConfig<Media, MediaDemoDto>()
			.Map(dest => dest.Id, src => src.Id.GetValue())
			.Map(dest => dest.PosterId, src => src.PosterId.GetValue())
			.Map(dest => dest.TrailerId, src => src.TrailerId.GetValue());
		
		config.NewConfig<Media, MediaDto>()
			.Map(dest => dest.PosterId, src => src.PosterId.GetValue())
			.Map(dest => dest.TrailerId, src => src.TrailerId.GetValue())
			.Map(dest => dest.PlatformIds, src => src.PlatformIds.Select(id => id.Value))
			.Map(dest => dest.ContentIds, src => src.ContentIds.Select(id => id.Value))
			.Map(dest => dest.GenreIds, src => src.GenreIds.Select(id => id.Value))
			.Map(dest => dest.LatestReview, src => src.Reviews.OrderByDescending(r => r.Date).FirstOrDefault());

		config.NewConfig<Movie, MovieDto>()
			.Map(dest => dest.SequelId, src => src.SequelId.GetValue())
			.Map(dest => dest.PrequelId, src => src.PrequelId.GetValue())
			.Map(dest => dest.PosterId, src => src.PosterId.GetValue())
			.Map(dest => dest.TrailerId, src => src.TrailerId.GetValue())
			.Map(dest => dest.PlatformIds, src => src.PlatformIds.Select(id => id.Value))
			.Map(dest => dest.ContentIds, src => src.ContentIds.Select(id => id.Value))
			.Map(dest => dest.GenreIds, src => src.GenreIds.Select(id => id.Value))
			.Map(dest => dest.LatestReview, src => src.Reviews.OrderByDescending(r => r.Date).FirstOrDefault());

		config.NewConfig<Series, SeriesDto>()
			.Map(dest => dest.PosterId, src => src.PosterId.GetValue())
			.Map(dest => dest.TrailerId, src => src.TrailerId.GetValue())
			.Map(dest => dest.PlatformIds, src => src.PlatformIds.Select(id => id.Value))
			.Map(dest => dest.ContentIds, src => src.ContentIds.Select(id => id.Value))
			.Map(dest => dest.GenreIds, src => src.GenreIds.Select(id => id.Value))
			.Map(dest => dest.LatestReview, src => src.Reviews.OrderByDescending(r => r.Date).FirstOrDefault());
		
		config.NewConfig<Review, ReviewDto>()
			.Map(dest => dest.UserId, src => src.UserId);

		config.NewConfig<Staff, PostStaffDto>()
			.Map(dest => dest.PersonId, src => src.PersonId.Value);
		
		config.NewConfig<Staff, StaffDto>()
			.Map(dest => dest.PersonId, src => src.PersonId.Value)
			.Map(dest => dest.Role, src => src.Role.ToString());
		
		config.NewConfig<Episode, EpisodeDto>()
			.Map(dest => dest.ContentIds, src => src.ContentIds.Select(id => id.Value));
	}
	
	private static short? GetStartYear(DateTimeOffset? date) => date is null ? null : (short?)date.Value.Year;
	
	
}