using UMS.API.Infrastructure.Middlewares;

namespace UMS.API.Infrastructure.Extensions;

public static class MiddlewareExtensions
{

    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
    {
        app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

        return app;
    }
}