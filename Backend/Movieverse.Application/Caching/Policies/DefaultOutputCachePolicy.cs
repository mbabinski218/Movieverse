using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Primitives;

namespace Movieverse.Application.Caching.Policies;

public sealed class DefaultOutputCachePolicy : IOutputCachePolicy
{
	public ValueTask CacheRequestAsync(OutputCacheContext context, CancellationToken cancellation)
	{
		var attemptOutputCaching = AttemptOutputCaching(context);
		
		context.EnableOutputCaching = true;
		context.AllowCacheLookup = attemptOutputCaching;
		context.AllowCacheStorage = attemptOutputCaching;
		context.AllowLocking = true;
        
		context.CacheVaryByRules.QueryKeys = "*";

		return ValueTask.CompletedTask;
	}

	public ValueTask ServeFromCacheAsync(OutputCacheContext context, CancellationToken cancellation) => ValueTask.CompletedTask;

	public ValueTask ServeResponseAsync(OutputCacheContext context, CancellationToken cancellation)
	{
		var response = context.HttpContext.Response;
        
		if (!StringValues.IsNullOrEmpty(response.Headers.SetCookie))
		{
			context.AllowCacheStorage = false;
			return ValueTask.CompletedTask;
		}
        
		switch (response.StatusCode)
		{
			case >= 200 and < 300 or 302: // 302 is a redirect and 200-299 are success codes
				context.AllowCacheStorage = true;
				return ValueTask.CompletedTask;
			default:
				context.AllowCacheStorage = false;
				return ValueTask.CompletedTask;
		}
	}
	
	private static bool AttemptOutputCaching(OutputCacheContext context)
	{
		var method = context.HttpContext.Request.Method;
		
        return HttpMethods.IsGet(method) || 
               HttpMethods.IsPost(method) || 
               HttpMethods.IsPut(method);
	}
}