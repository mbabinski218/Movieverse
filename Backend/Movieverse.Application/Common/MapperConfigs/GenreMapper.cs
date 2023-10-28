using Mapster;
using Movieverse.Application.Common.Extensions;
using Movieverse.Contracts.DataTransferObjects.Genre;
using Movieverse.Domain.AggregateRoots;

namespace Movieverse.Application.Common.MapperConfigs;

public sealed class GenreMapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<Genre, GenreDto>()
			.Map(dest => dest.Id, src => src.Id.GetValue())
			.Map(dest => dest.Name, src => src.Name);
	}
}