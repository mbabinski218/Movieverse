﻿using System.Text.Json;

namespace Movieverse.API.Common.Middlewares;

public sealed class ExceptionHandlingMiddleware : IMiddleware
{
	private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (NotImplementedException)
        {
            context.Response.StatusCode = StatusCodes.Status501NotImplemented;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(CreateJsonResponse("Not implemented yet."));
        }
        catch (Exception ex)
        {
            var query = string.Join(", ", context.Request.Query.Keys);
            var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
            
            _logger.LogCritical("Request: {request} | Query: {query} | Body: {body} | Exception: {msg}",
                context.Request.Path.ToString(), query, body, ex.Message);
            
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(CreateJsonResponse("An unexpected error occurred."));
        }
    }
    
    private static string CreateJsonResponse(string message) => JsonSerializer.Serialize(new List<string>{ new(message) });
}