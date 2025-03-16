using System.Net;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Localization;
using Serilog;
using UMS.API.Infrastructure.Utilities;
using UMS.Domain.Resources;

namespace UMS.API.Infrastructure.Middlewares;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IStringLocalizer<ErrorMessages> _localizer;

    public GlobalExceptionHandlingMiddleware(RequestDelegate requestDelegate, IStringLocalizer<ErrorMessages> localizer)
    {
        _next = requestDelegate;
        _localizer = localizer;
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
        var (statusCode, response) = ExceptionHandler.Handle(exception, _localizer);

        var jsonSerializer = new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var serializedException = JsonSerializer.Serialize(response, jsonSerializer);
        
        if (statusCode == (int)HttpStatusCode.InternalServerError)
        {
            Log.Fatal(
                "Critical error occured - {0} \n {1} \n {2}",
                exception.Message,
                exception.StackTrace, 
                exception.InnerException);
        }
        else
        {
            Log.Error(
                "Error occured - {0} \n {1} \n {2}",
                exception.Message,
                exception.StackTrace,
                exception.InnerException);
        }

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = MediaTypeNames.Application.Json;

        await httpContext.Response.WriteAsync(serializedException);
    }
}