using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;

namespace Movieverse.Application.Metrics;

public sealed class MetricsMiddleware : IMiddleware
{
	private readonly IMetricsService _metricsService;

	public MetricsMiddleware(IMetricsService metricsService)
	{
		_metricsService = metricsService;
	}

	public Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		if (!TryGetMetrics(context, out var metrics))
		{
			return next(context);
		}

		var additionalAttribute = metrics.AdditionalAttributeType switch
		{
			AdditionalAttributeType.Id => context.Request.RouteValues["Id"] as string,
			AdditionalAttributeType.Query => context.Request.QueryString.ToString(),
			AdditionalAttributeType.Path => context.Request.Path.ToString(),
			_ => null
		};
		var name = $"{metrics.Name} {additionalAttribute}";
		
		switch (metrics.MetricsType)
		{
			case MetricsType.Counter:
				_metricsService.CounterLogic(name);
				break;
			case MetricsType.Histogram:
				_metricsService.HistogramLogic(name);
				break;
			default:
				return next(context);
		}
		
		return next(context);
	}
	
	private static bool TryGetMetrics(HttpContext httpContext, [MaybeNullWhen(false)] out Meter meter)
	{
		var attribute = httpContext.GetEndpoint()
			?.Metadata
			.GetMetadata<MetricsAttribute>();

		if (attribute is null)
		{
			meter = null!;
			return false;
		}
		
		meter = attribute.BuildMetrics();
		return true;
	}
}