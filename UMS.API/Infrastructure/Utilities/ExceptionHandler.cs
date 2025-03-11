using System.Net;
using UMS.API.Models.Response;

namespace UMS.API.Infrastructure.Utilities;

public static class ExceptionHandler
{
    private static readonly Dictionary<Type, Func<Exception, ExceptionHandlerResponse>> _handlers =
        new();

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
}