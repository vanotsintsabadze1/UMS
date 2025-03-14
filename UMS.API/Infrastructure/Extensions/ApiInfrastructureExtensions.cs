using System.Text.Json.Serialization;

namespace UMS.API.Infrastructure.Extensions;

public static class ApiInfrastructureExtensions
{
    public static IServiceCollection ConfigureControllers(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions
                    .Converters
                    .Add(new JsonStringEnumConverter());
            });

        return services;
    }

    public static IServiceCollection ConfigureRouting(this IServiceCollection services)
    {
        services.AddRouting(options => options.LowercaseUrls = true);
        
        return services;
    }
}