using Mapster;
using Movieverse.Application.Common.Extensions;
using Movieverse.Contracts.DataTransferObjects.Content;
using Movieverse.Domain.AggregateRoots;

namespace Movieverse.Application.Common.MapperConfigs;

public sealed class ContentMapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<Content, ContentInfoDto>()
			.Map(dest => dest.Id, src => src.Id.GetValue());
	}
}