using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Serilog;
using UMS.API.Infrastructure.Utilities;

namespace UMS.API.Infrastructure.Middlewares;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionHandlingMiddleware(RequestDelegate requestDelegate)
    {
        _next = requestDelegate;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var (statusCode, response) = ExceptionHandler.Handle(exception);

        var jsonSerializer = new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var serializedException = JsonSerializer.Serialize(response, jsonSerializer);
        
        if (statusCode == (int)HttpStatusCode.InternalServerError)
        {
            Log.Fatal(
                "Critical error occured - {0} \n - {1}", 
                exception.StackTrace, 
                exception.InnerException);
        }
        else
        {
            Log.Error(
                "Error occured - {0} \n - {1}",
                exception.StackTrace,
                exception.InnerException);
        }

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";

        await httpContext.Response.WriteAsync(serializedException);
    }
}