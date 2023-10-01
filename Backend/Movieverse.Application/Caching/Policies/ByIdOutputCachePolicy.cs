using Microsoft.AspNetCore.OutputCaching;

namespace Movieverse.Application.Caching.Policies;

public sealed class ByIdOutputCachePolicy : IOutputCachePolicy
{
	public ValueTask CacheRequestAsync(OutputCacheContext context, CancellationToken cancellation)
	{
		var idRouteValue = context.HttpContext.Request.RouteValues["id"];
		
		if (idRouteValue is null)
		{
			return ValueTask.CompletedTask;
		}
		
		context.Tags.Add(idRouteValue.ToString()!);

		return ValueTask.CompletedTask;
	}

	public ValueTask ServeFromCacheAsync(OutputCacheContext context, CancellationToken cancellation) => ValueTask.CompletedTask;

	public ValueTask ServeResponseAsync(OutputCacheContext context, CancellationToken cancellation) => ValueTask.CompletedTask;
}