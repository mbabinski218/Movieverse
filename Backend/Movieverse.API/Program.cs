using Movieverse.API.Common;
using Movieverse.API.Services;
using NLog.Extensions.Logging;
using Movieverse.Infrastructure;
using Movieverse.Application;
using Movieverse.Application.Interfaces;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

builder.Logging.ClearProviders();
builder.Logging.AddNLog();

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddApplication(builder.Configuration);
services.AddInfrastructure(builder.Configuration);

services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
services.AddScoped<IHttpService, HttpService>();

services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.SeedDatabase();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
