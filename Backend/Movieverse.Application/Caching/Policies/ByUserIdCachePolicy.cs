using Microsoft.AspNetCore.OutputCaching;
using Movieverse.Application.Interfaces;

namespace Movieverse.Application.Caching.Policies;

public sealed class ByUserIdCachePolicy : IOutputCachePolicy
{
	private readonly IHttpService _httpService;

	public ByUserIdCachePolicy(IHttpService httpService)
	{
		_httpService = httpService;
	}
	
	public ValueTask CacheRequestAsync(OutputCacheContext context, CancellationToken cancellation)
	{
		var userId = _httpService.UserId;
		
		if (userId is null)
		{
			return ValueTask.CompletedTask;
		}
		
		context.Tags.Add(userId.Value.ToString());

		return ValueTask.CompletedTask;
	}

	public ValueTask ServeFromCacheAsync(OutputCacheContext context, CancellationToken cancellation) => ValueTask.CompletedTask;

	public ValueTask ServeResponseAsync(OutputCacheContext context, CancellationToken cancellation) => ValueTask.CompletedTask;
}