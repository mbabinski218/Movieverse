using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Movieverse.Consumer;
using NLog.Extensions.Logging;
using Movieverse.Consumer.Common.Settings;
using Movieverse.Consumer.Interfaces;
using Movieverse.Consumer.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;
var logging = builder.Logging;

var defaultSettings = new DefaultSettings();
configuration.Bind(DefaultSettings.key, defaultSettings);

logging.ClearProviders();
logging.AddNLog();

services.AddMassTransit(configuration);

services.Configure<EmailServiceSettings>(configuration.GetSection(EmailServiceSettings.key));

services.AddScoped<IEmailServiceProvider, EmailServiceProvider>();

var app = builder.Build();

app.MapHealthChecks(defaultSettings.HealthCheckEndpoint, new HealthCheckOptions
{
	ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();