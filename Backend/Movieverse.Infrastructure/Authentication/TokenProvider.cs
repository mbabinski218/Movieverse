using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Movieverse.Application.Common.Settings;
using Movieverse.Application.Interfaces;

namespace Movieverse.Infrastructure.Authentication;

public sealed class TokenProvider : ITokenProvider
{
	private readonly string _secret;
	private readonly string _issuer;
	private readonly string _audience;
	private readonly TimeSpan _tokenExpirationTime;

	public TokenProvider(IOptions<AuthenticationSettings> authenticationSettings)
	{
		ArgumentNullException.ThrowIfNull(authenticationSettings.Value, nameof(authenticationSettings));
		
		_secret = authenticationSettings.Value.Token.Secret;
		_issuer = authenticationSettings.Value.Token.Issuer;
		_audience = authenticationSettings.Value.Token.Audience;
		_tokenExpirationTime = TimeSpan.Parse(authenticationSettings.Value.Token.TokenExpirationTime);
	}

	public string GenerateAccessToken(IEnumerable<Claim> claims)
	{
		var signingCredentials = new SigningCredentials(
			new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret)), 
			SecurityAlgorithms.HmacSha512Signature);

		var token = new JwtSecurityToken
		(
			issuer: _issuer,
			audience: _audience,
			claims: claims,
			expires: DateTime.Now.Add(_tokenExpirationTime),
			signingCredentials: signingCredentials
		);

		var tokenHandler = new JwtSecurityTokenHandler();
		return tokenHandler.WriteToken(token);
	}

	public string GenerateRefreshToken()
	{
		return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
	}
}