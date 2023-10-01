using Microsoft.Extensions.Options;
using Movieverse.Application.Common.Settings;
using Movieverse.Contracts.DataTransferObjects.User;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Infrastructure.Authentication;

public sealed class FacebookAuthentication
{
	private readonly string _appId;
	private readonly string _appSecret;

	public FacebookAuthentication(IOptions<AuthenticationSettings> authenticationSettings)
	{
		ArgumentNullException.ThrowIfNull(authenticationSettings.Value, nameof(authenticationSettings));
		_appId = authenticationSettings.Value.Facebook.AppId;
		_appSecret = authenticationSettings.Value.Facebook.AppSecret;
	}

	public Task<Result<TokensDto>> AuthenticateAsync(string idToken)
	{
		throw new NotImplementedException();
	}
}