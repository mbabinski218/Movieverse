using Microsoft.AspNetCore.Http.Extensions;
using Movieverse.Application.Interfaces;

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
}