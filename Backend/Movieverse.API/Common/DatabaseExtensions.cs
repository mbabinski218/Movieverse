using Movieverse.Application.Interfaces;

namespace Movieverse.API.Common;

public static class DatabaseExtensions
{
	public static async Task<IApplicationBuilder> SeedDatabase(this IApplicationBuilder app)
	{
		ArgumentNullException.ThrowIfNull(app, nameof(app));
		
		using var scope = app.ApplicationServices.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<IDatabaseSeeder>();
        await seeder.SeedAsync();
		
		return app;
	}
}