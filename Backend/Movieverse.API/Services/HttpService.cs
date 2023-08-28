using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http.Extensions;
using Movieverse.Application.Interfaces;
using Movieverse.Domain.Common.Types;
using Movieverse.Domain.ValueObjects.Id;

namespace Movieverse.API.Services;

public sealed class HttpService : IHttpService
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public HttpService(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	public Uri? Uri
	{
		get
		{
			var url = _httpContextAccessor.HttpContext?.Request.GetDisplayUrl();
			return url is null ? null : new Uri(url);
		}
	}
	
	public string? AccessToken
	{
		get
		{
			var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"]
				.FirstOrDefault()
				?.Split(" ")
				.LastOrDefault();

			return token;
		}
	}
	
	public AggregateRootId? UserId
	{
		get
		{
			var token = AccessToken;

			if (token is null)
			{
				return null;
			}
		
			var handler = new JwtSecurityTokenHandler();
		
			var decodedToken = handler.CanReadToken(token) ? handler.ReadJwtToken(token) : null;
			
			var id = decodedToken?.Claims.FirstOrDefault(c => c.Type == ClaimNames.id)?.Value;
			return id is null ? null : AggregateRootId.Create(id);
		}
	}
}