using Microsoft.AspNetCore.OutputCaching;

namespace Movieverse.Application.Caching.Policies;

public sealed class ByQueryCachePolicy : IOutputCachePolicy
{
	public ValueTask CacheRequestAsync(OutputCacheContext context, CancellationToken cancellation)
	{
		var query = context.HttpContext.Request.QueryString.Value;
		
		if (query is null)
		{
			return ValueTask.CompletedTask;
		}
		
		context.Tags.Add(query);

		return ValueTask.CompletedTask;
	}

	public ValueTask ServeFromCacheAsync(OutputCacheContext context, CancellationToken cancellation) => ValueTask.CompletedTask;

	public ValueTask ServeResponseAsync(OutputCacheContext context, CancellationToken cancellation) => ValueTask.CompletedTask;
}