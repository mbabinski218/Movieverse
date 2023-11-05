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
			.Map(dest => dest.TrailerId, src => src.TrailerId.GetValue())
			.Map(dest => dest.Rating, src => src.BasicStatistics.Rating);

		config.NewConfig<Media, MediaDto>()
			.Map(dest => dest.PosterId, src => src.PosterId.GetValue())
			.Map(dest => dest.TrailerId, src => src.TrailerId.GetValue());

		config.NewConfig<Movie, MovieDto>()
			.Map(dest => dest.SequelId, src => src.SequelId.GetValue())
			.Map(dest => dest.PrequelId, src => src.PrequelId.GetValue())
			.Map(dest => dest.PosterId, src => src.PosterId.GetValue())
			.Map(dest => dest.TrailerId, src => src.TrailerId.GetValue());

		config.NewConfig<Series, SeriesDto>()
			.Map(dest => dest.PosterId, src => src.PosterId.GetValue())
			.Map(dest => dest.TrailerId, src => src.TrailerId.GetValue());
		
		config.NewConfig<Review, ReviewDto>()
			.Map(dest => dest.UserId, src => src.UserId);

		config.NewConfig<Staff, PostStaffDto>()
			.Map(dest => dest.PersonId, src => src.PersonId.Value);
		
		config.NewConfig<Staff, StaffDto>()
			.Map(dest => dest.PersonId, src => src.PersonId.Value)
			.Map(dest => dest.Role, src => src.Role.ToString());
		
		// config.NewConfig<Episode, EpisodeDto>()
		// 	.Map(dest => dest.ContentIds, src => src.ContentIds.Select(id => id.Value));

		config.NewConfig<Media, SearchMediaDto>()
			.Map(dest => dest.Id, src => src.Id.GetValue())
			.Map(dest => dest.Title, src => src.Title)
			.Map(dest => dest.ReleaseDate, src => src.Details.ReleaseDate)
			.Map(dest => dest.Poster, src => src.PosterId.GetValue().ToString())
			.Map(dest => dest.Description, src => GetDescription(src.Details.Storyline));
		
		config.NewConfig<PaginatedList<Media>, PaginatedList<MediaDemoDto>>()
			.Map(dest => dest.Items, src => src.Items.Adapt<List<MediaDemoDto>>());
	}
	
	private static short? GetStartYear(DateTimeOffset? date) => date is null ? null : (short?)date.Value.Year;
	
	private static DateTime? ToDateTime(DateTimeOffset? date) => date?.DateTime;

	private static string? GetDescription(string? description)
	{
		if (description is null)
		{
			return null;
		}
		
		if (description.Length <= 100)
		{
			return description + "...";
		}
		
		return description[..100] + "...";
	}
}