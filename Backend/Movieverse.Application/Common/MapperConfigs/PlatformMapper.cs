using Mapster;
using Movieverse.Application.Common.Extensions;
using Movieverse.Contracts.DataTransferObjects.Platform;
using Movieverse.Domain.AggregateRoots;

namespace Movieverse.Application.Common.MapperConfigs;

public sealed class PlatformMapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<Platform, PlatformDto>()
			.Map(dest => dest.Id, src => src.Id.GetValue());

		config.NewConfig<Platform, PlatformDemoDto>()
			.Map(dest => dest.Id, src => src.Id.GetValue());
		
		config.NewConfig<Platform, PlatformInfoDto>()
			.Map(dest => dest.Id, src => src.Id.GetValue());
	}
}