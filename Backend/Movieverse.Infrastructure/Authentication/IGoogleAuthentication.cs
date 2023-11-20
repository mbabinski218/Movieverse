using Google.Apis.Auth;
using Movieverse.Domain.AggregateRoots;
using Movieverse.Domain.Common.Result;

namespace Movieverse.Infrastructure.Authentication;

public interface IGoogleAuthentication
{
	Task<Result<GoogleUser>> AuthenticateAsync(string idToken);
	User GetUser(GoogleJsonWebSignature.Payload payload);
	bool IsEmailVerified(GoogleJsonWebSignature.Payload payload);
}