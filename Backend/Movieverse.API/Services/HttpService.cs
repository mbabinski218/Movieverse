﻿using System.IdentityModel.Tokens.Jwt;
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

	private bool _uriWasSet;
	private Uri? _uri;
	public Uri? Uri
	{
		get
		{
			if (_uriWasSet)
			{
				return _uri;
			}
			
			var url = _httpContextAccessor.HttpContext?.Request.GetDisplayUrl();
			_uri = url is null ? null : new Uri(url);
			
			_uriWasSet = true;
			return _uri;
		}
	}

	private bool _idHeaderWasSet;
	private Guid? _idHeader;
	public Guid? IdHeader
	{
		get
		{
			if (_idHeaderWasSet)
			{
				return _idHeader;
			}

			var idRouteValue = _httpContextAccessor.HttpContext?.Request.RouteValues["Id"];
			_idHeader = idRouteValue is not string id ? null : Guid.Parse(id);

			_idHeaderWasSet = true;
			return _idHeader;
		}
	}

	private bool _accessTokenWasSet;
	private string? _accessToken;
	public string? AccessToken
	{
		get
		{
			if (_accessTokenWasSet)
			{
				return _accessToken;
			}
			
			_accessToken = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"]
				.FirstOrDefault()
				?.Split(" ")
				.LastOrDefault();
			
			_accessTokenWasSet = true;
			return _accessToken;
		}
	}
	
	private bool _userIdWasSet;
	private Guid? _userId;
	public Guid? UserId
	{
		get
		{
			if (_userIdWasSet)
			{
				return _userId;
			}
			
			var token = AccessToken;

			if (token is null)
			{
				_userId = null;
				return null;
			}
		
			var handler = new JwtSecurityTokenHandler();
		
			var decodedToken = handler.CanReadToken(token) ? handler.ReadJwtToken(token) : null;
			
			var id = decodedToken?.Claims.FirstOrDefault(c => c.Type == ClaimNames.id)?.Value;
			_userId = id is null ? null : Guid.Parse(id);
			
			_userIdWasSet = true;
			return _userId;
		}
	}

	private bool _roleWasSet;
	private UserRole[]? _role;
	public UserRole[]? Role
	{
		get
		{
			if (_roleWasSet)
			{
				return _role;
			}

			var token = AccessToken;

			if (token is null)
			{
				_role = null;
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
				_role = null;
				_roleWasSet = true;
				return _role;
			}
			
			var userRoles = new List<UserRole>();
			foreach (var role in roles)
			{
				if (Enum.TryParse<UserRole>(role, out var userRole))
				{
					userRoles.Add(userRole);
				}
			}
			
			_role = userRoles.ToArray();
			_roleWasSet = true;
			return _role;
		}
	}
}