namespace UMS.API.Infrastructure.Extensions;

public static class LocalizationExtensions
{
    public static IApplicationBuilder UseConfiguredLocalization(this IApplicationBuilder app)
    {
        app.UseRequestLocalization(
            new RequestLocalizationOptions()
                .SetDefaultCulture("en-US")
                .AddSupportedCultures("en-US", "ka-GE")
                .AddSupportedUICultures("en-US", "ka-GE"));

        return app;
    }
}