using Mapster;
using Movieverse.Application.Common.Extensions;
using Movieverse.Contracts.DataTransferObjects.User;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Entities;

namespace Movieverse.Application.Common.MapperConfigs;

public sealed class UserMapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<User, UserDto>()
			.Map(dest => dest.UserName, src => src.UserName)
			.Map(dest => dest.Email, src => src.Email)
			.Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
			.Map(dest => dest.Information, src => src.Information)
			.Map(dest => dest.AvatarPath, src => src.AvatarId.GetValue())
			.Map(dest => dest.EmailConfirmed, src => src.EmailConfirmed)
			.Map(dest => dest.PersonId, src => src.PersonId.GetValue());

		config.NewConfig<MediaInfo, WatchlistStatusDto>()
			.Map(dest => dest.MediaId, src => src.MediaId.GetValue())
			.Map(dest => dest.IsOnWatchlist, src => src.IsOnWatchlist);
	}
}