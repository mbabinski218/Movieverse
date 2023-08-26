using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Movieverse.Application.Common.Settings;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Result;
using Movieverse.Infrastructure.Persistence;

namespace Movieverse.Infrastructure.Authentication;

public readonly struct GoogleUser
{
	public bool IsRegistered { get; init; }
	public GoogleJsonWebSignature.Payload Payload { get; init; }

	public GoogleUser(bool isRegistered, GoogleJsonWebSignature.Payload payload)
	{
		IsRegistered = isRegistered;
		Payload = payload;
	}
}

public sealed class GoogleAuthentication
{
	private readonly GoogleJsonWebSignature.ValidationSettings _validationSettings;
	private readonly AppDbContext _dbContext;

	public GoogleAuthentication(IOptions<AuthenticationSettings> googleSettings, AppDbContext dbContext)
	{
		_dbContext = dbContext;
		ArgumentNullException.ThrowIfNull(googleSettings.Value, nameof(googleSettings));

		_validationSettings = new GoogleJsonWebSignature.ValidationSettings
		{
			Audience = new List<string> { googleSettings.Value.Google.ClientId }
		};
	}

	public async Task<Result<GoogleUser>> AuthenticateAsync(string idToken)
	{
		var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, _validationSettings);
		if (payload is null)
		{
			return Error.Unauthorized("Failed to authenticate via Google");
		}

		return await _dbContext.Users.AnyAsync(x => x.Email == payload.Email)
			? new GoogleUser(false, payload)
			: new GoogleUser(true, payload);
	}

	public User GetUser(GoogleJsonWebSignature.Payload payload) =>
		User.Create(
			payload.Email,
			payload.Email.Split('@')[0],
			payload.GivenName,
			payload.FamilyName,
			0);


	public bool IsEmailVerified(GoogleJsonWebSignature.Payload payload) => payload.EmailVerified;
}