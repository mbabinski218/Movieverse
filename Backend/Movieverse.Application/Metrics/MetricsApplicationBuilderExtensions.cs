using Microsoft.AspNetCore.Builder;

namespace Movieverse.Application.Metrics;

public static class MetricsApplicationBuilderExtensions
{
	public static IApplicationBuilder UseMetrics(this IApplicationBuilder app)
	{
		ArgumentNullException.ThrowIfNull(app);

		return app.UseMiddleware<MetricsMiddleware>();
	}
}