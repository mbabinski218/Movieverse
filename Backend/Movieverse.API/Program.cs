using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Movieverse.API.Common;
using NLog.Extensions.Logging;
using Movieverse.API.Common.Extensions;
using Movieverse.API.Common.Middlewares;
using Movieverse.API.Common.Settings;
using Movieverse.API.Services;
using Movieverse.Application;
using Movieverse.Application.Common.Extensions;
using Movieverse.Application.Interfaces;
using Movieverse.Application.Metrics;
using Movieverse.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;
var logging = builder.Logging;

var defaultSettings = configuration.Map<DefaultSettings>();

logging.ClearProviders();
logging.AddNLog();

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddApplication(configuration);
services.AddInfrastructure(configuration, defaultSettings.DatabaseName);

services.AddHttpContextAccessor();
services.AddSingleton<IHttpService, HttpService>();
services.AddSingleton<ExceptionHandlingMiddleware>();

services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

const string cors = "corsapp";
services.AddCors(options => options.AddPolicy(cors, corsBuilder =>
    corsBuilder.WithOrigins(defaultSettings.Routes.Origin).AllowAnyMethod().AllowAnyHeader()));
    // corsBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.SeedDatabase();

app.UseRouting();

app.UseCors(cors);
app.UseMetrics();
app.UseOutputCache();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseRequestLocalization(options =>
{
    options.SupportedCultures = defaultSettings.SupportedCultures;
    options.SupportedUICultures = defaultSettings.SupportedCultures;
    options.RequestCultureProviders.Insert(0, new AppRequestCultureProvider(defaultSettings.Culture));
});

app.UseAuthentication();

app.UseHttpsRedirection();
app.MapHealthChecks(defaultSettings.Routes.HealthCheckEndpoint, new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
}).CacheOutput(policy => policy.NoCache());

app.UseAuthorization();
app.MapControllers();

app.Run();