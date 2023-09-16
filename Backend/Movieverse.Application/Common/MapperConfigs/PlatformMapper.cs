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
			.Map(dest => dest.Id, src => src.Id.Value)
			.Map(dest => dest.Name, src => src.Name)
			.Map(dest => dest.Price, src => src.Price)
			.Map(dest => dest.LogoId, src => src.LogoId.GetValue());
	}
}