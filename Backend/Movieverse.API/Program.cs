using Movieverse.API.Common.Extensions;
using Movieverse.API.Common.Middlewares;
using Movieverse.API.Services;
using NLog.Extensions.Logging;
using Movieverse.Infrastructure;
using Movieverse.Application;
using Movieverse.Application.Interfaces;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var service = builder.Services;
var logging = builder.Logging;

logging.ClearProviders();
logging.AddNLog();

service.AddControllers();
service.AddEndpointsApiExplorer();
service.AddSwaggerGen();

service.AddInfrastructure(configuration);
service.AddApplication(configuration);

service.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
service.AddScoped<IHttpService, HttpService>();
builder.Services.AddScoped<ExceptionHandlingMiddleware>();

service.Configure<RouteOptions>(options =>
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

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();