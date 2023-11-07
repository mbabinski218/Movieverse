using Mapster;
using Movieverse.Application.Common.Extensions;
using Movieverse.Contracts.DataTransferObjects.Person;
using Movieverse.Domain.AggregateRoots;

namespace Movieverse.Application.Common.MapperConfigs;

public sealed class PersonMapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<Person, SearchPersonDto>()
			.Map(dest => dest.Id, src => src.Id.GetValue())
			.Map(dest => dest.FullName, src => src.Information.FirstName + " " + src.Information.LastName)
			.Map(dest => dest.Age, src => src.Information.Age)
			.Map(dest => dest.Picture, src => src.PictureId.GetValue())
			.Map(dest => dest.Biography, src => src.Biography);

		config.NewConfig<Person, PersonInfoDto>()
			.Map(dest => dest.Id, src => src.Id.GetValue())
			.Map(dest => dest.PictureId, src => src.PictureId.GetValue())
			.Map(dest => dest.FirstName, src => src.Information.FirstName)
			.Map(dest => dest.LastName, src => src.Information.LastName);
	}
}