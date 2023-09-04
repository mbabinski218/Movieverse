using Mapster;
using Movieverse.Application.Common.Extensions;
using Movieverse.Contracts.DataTransferObjects.Media;
using Movieverse.Domain.AggregateRoots.Media;

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
		
		// config.NewConfig<Series, MediaInfoDto>()
		// 	.Map(dest => dest.Type, src => nameof(Series));
		
		config.NewConfig<Media, MediaShortInfoDto>()
			.Map(dest => dest.Title, src => src.Title)
			.Map(dest => dest.Certificate, src => src.Details.Certificate)
			.Map(dest => dest.Id, src => src.Id)
			.Map(dest => dest.PosterId, src => src.PosterId.GetValue())
			.Map(dest => dest.Rating, src => src.BasicStatistics.Rating)
			.Map(dest => dest.StartYear, src => GetStartYear(src.Details.ReleaseDate));
	}
	
	private static short? GetStartYear(DateTimeOffset? date) => date is null ? null : (short?)date.Value.Year;
	
	
}