﻿using System.Text.Json;
using FluentValidation;
using Movieverse.Application.Common.Exceptions;

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
        catch (ResultException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            
            await context.Response.WriteAsync(CreateJsonResponse(ex.Message));
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            
            var errors = ex.Errors.Select(vf => vf.ErrorMessage);
            await context.Response.WriteAsync(CreateJsonResponse(errors));
        }
        catch (Exception ex)
        {
            var query = string.Join(", ", context.Request.Query.Keys);
            var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
            
            _logger.LogCritical("Request: {request} | Query: {query} | Body: {body} | Exception: {msg} | StackTrace: {stack}",
                context.Request.Path.ToString(), query, body, ex.Message, ex.StackTrace);
            
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(CreateJsonResponse("An unexpected error occurred. Try again later."));
        }
    }
    
    private static string CreateJsonResponse(IEnumerable<string> messages) => JsonSerializer.Serialize(messages);
    private static string CreateJsonResponse(string message) => JsonSerializer.Serialize(new List<string>{ new(message) });
}