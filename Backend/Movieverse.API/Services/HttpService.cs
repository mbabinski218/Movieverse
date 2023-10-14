using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http.Extensions;
using Movieverse.Application.Interfaces;
using Movieverse.Domain.Common.Types;

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
			try
			{
				var url = _httpContextAccessor.HttpContext?.Request.GetDisplayUrl();
				return url is null ? null : new Uri(url);
			}
			catch
			{
				return null;
			}
		}
	}
	
	public Guid? IdHeader
	{
		get
		{
			try
			{
				var idRouteValue = _httpContextAccessor.HttpContext?.Request.RouteValues["Id"];
				return idRouteValue is not string id ? null : Guid.Parse(id);
			}
			catch
			{
				return null;
			}
		}
	}

	public string? AccessToken
	{
		get
		{
			try
			{
				return _httpContextAccessor.HttpContext?.Request.Headers["Authorization"]
					.FirstOrDefault()
					?.Split(" ")
					.LastOrDefault();
			}
			catch
			{
				return null;
			}
		}
	}
	
	public Guid? UserId
	{
		get
		{
			try
			{
				var token = AccessToken;

				if (token is null)
				{
					return null;
				}
		
				var handler = new JwtSecurityTokenHandler();
		
				var decodedToken = handler.CanReadToken(token) ? handler.ReadJwtToken(token) : null;
			
				var id = decodedToken?.Claims.FirstOrDefault(c => c.Type == ClaimNames.id)?.Value;
				return id is null ? null : Guid.Parse(id);
			}
			catch
			{
				return null;
			}
		}
	}

	public UserRole[]? Role
	{
		get
		{
			try
			{
				var token = AccessToken;

				if (token is null)
				{
					return null;
				}
		
				var handler = new JwtSecurityTokenHandler();
		
				var decodedToken = handler.CanReadToken(token) ? handler.ReadJwtToken(token) : null;

				var roles = decodedToken?.Claims
					.Where(c => c.Type == ClaimNames.role)
					.Select(c => c.Value)
					.ToList();
			
				if (roles is null)
				{
					return null;
				}
			
				var userRoles = new List<UserRole>();
				foreach (var role in roles)
				{
					if (Enum.TryParse<UserRole>(role, out var userRole))
					{
						userRoles.Add(userRole);
					}
				}
			
				return userRoles.ToArray();
			}
			catch
			{
				return null;
			}
		}
	}
}