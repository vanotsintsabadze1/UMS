using Microsoft.Extensions.DependencyInjection;
using UMS.Application.Interfaces.UOW;
using UMS.Infrastructure.UOW;
using UMS.Persistence.Context;

namespace UMS.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        return services;
    }
}