using System.Security.Claims;

namespace Movieverse.Application.Interfaces;

public interface ITokenProvider
{
	string GenerateAccessToken(IEnumerable<Claim> claims);
	string GenerateRefreshToken();
}