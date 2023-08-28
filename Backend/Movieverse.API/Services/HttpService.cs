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
	
	public AggregateRootId? UserId
	{
		get
		{
			var id = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimNames.id)?.Value;
			return id is null ? null : AggregateRootId.Create(id);
		}
	}
}