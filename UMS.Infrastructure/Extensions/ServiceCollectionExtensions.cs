using Microsoft.Extensions.DependencyInjection;
using UMS.Persistence.Context;

namespace UMS.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>();

        return services;
    }
}