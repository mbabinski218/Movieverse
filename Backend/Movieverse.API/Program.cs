using Movieverse.API.Common;
using NLog.Extensions.Logging;
using Movieverse.Infrastructure;
using Movieverse.Application;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

builder.Logging.ClearProviders();
builder.Logging.AddNLog();

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddApplication();
services.AddInfrastructure(builder.Configuration);

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
