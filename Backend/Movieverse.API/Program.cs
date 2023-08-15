using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using NLog.Extensions.Logging;
using Movieverse.API.Common.Extensions;
using Movieverse.API.Common.Middlewares;
using Movieverse.API.Common.Settings;
using Movieverse.API.Services;
using Movieverse.Application;
using Movieverse.Application.Interfaces;
using Movieverse.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;
var logging = builder.Logging;

var defaultSettings = new DefaultSettings();
configuration.Bind(DefaultSettings.key, defaultSettings);

logging.ClearProviders();
logging.AddNLog();

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddApplication(configuration);
services.AddInfrastructure(configuration, defaultSettings.DatabaseName);

services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
services.AddScoped<IHttpService, HttpService>();
services.AddScoped<ExceptionHandlingMiddleware>();

services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.SeedDatabase();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.MapHealthChecks(defaultSettings.Routes.HealthCheckEndpoint, new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.UseAuthorization();
app.MapControllers();
app.Run();