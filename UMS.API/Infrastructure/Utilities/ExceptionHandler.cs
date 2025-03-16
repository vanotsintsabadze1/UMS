using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Localization;
using UMS.API.Models.Response;
using UMS.Application.Exceptions;
using UMS.Domain.Resources;

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

    public static ExceptionHandlerResponse Handle(Exception ex, IStringLocalizer<ErrorMessages> localizer)
    {
        return _handlers.TryGetValue(ex.GetType(), out var handler)
            ? handler(ex)
            : HandleDefault(ex, localizer);
    }

    private static ExceptionHandlerResponse HandleDefault(Exception ex, IStringLocalizer<ErrorMessages> localizer)
    {
        return new ExceptionHandlerResponse(
            (int)HttpStatusCode.InternalServerError,
            new ApiResponse(localizer[ErrorMessageNames.UnexpectedError]));
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