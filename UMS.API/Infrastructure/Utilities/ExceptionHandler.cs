using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using UMS.API.Models.Response;
using UMS.Application.Exceptions;

namespace UMS.API.Infrastructure.Utilities;

public static class ExceptionHandler
{
    private static readonly Dictionary<Type, Func<Exception, ExceptionHandlerResponse>> _handlers =
        new()
        {
            { typeof(ConflictException), (exception) => HandleConflictException((ConflictException)exception)},
            { typeof(BadRequestException), (exception) => HandleBadRequestException((BadRequestException)exception)},
            { typeof(NotFoundException), (exception) => HandleNotFoundException((NotFoundException)exception)}
        };

    public static ExceptionHandlerResponse Handle(Exception ex)
    {
        return _handlers.TryGetValue(ex.GetType(), out var handler)
            ? handler(ex)
            : HandleDefault(ex);
    }

    private static ExceptionHandlerResponse HandleDefault(Exception ex)
    {
        return new ExceptionHandlerResponse(
            (int)HttpStatusCode.InternalServerError,
            new ApiResponse("Unexpected error happened on the server while trying to serve the response"));
    }

    private static ExceptionHandlerResponse HandleConflictException(ConflictException exception)
    {
        return new ExceptionHandlerResponse(
            (int)HttpStatusCode.Conflict,
            new ApiResponse(exception.Message));
    }

    private static ExceptionHandlerResponse HandleBadRequestException(BadRequestException exception)
    {
        return new ExceptionHandlerResponse(
            (int)HttpStatusCode.BadRequest,
            new ApiResponse(exception.Message));
    }

    private static ExceptionHandlerResponse HandleNotFoundException(NotFoundException exception)
    {
        return new ExceptionHandlerResponse(
            (int)HttpStatusCode.NotFound,
            new ApiResponse(exception.Message));
    }
}