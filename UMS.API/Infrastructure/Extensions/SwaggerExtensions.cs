using Microsoft.OpenApi.Models;

namespace UMS.API.Infrastructure.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "User Management System API",
                Version = "v1",
                Description = "User Management System for TBC Pay Technical Task",
                Contact = new OpenApiContact()
                {
                    Name = "Vano Tsintsabadze",
                    Email = "tsintsabadzevano@gmail.com"
                }
            });
        });

        return services;
    }

    public static IApplicationBuilder UseConfiguredSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }
}