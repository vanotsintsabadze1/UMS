using Microsoft.Extensions.DependencyInjection;
using UMS.Application.Interfaces;
using UMS.Application.Interfaces.Repositories;
using UMS.Application.Interfaces.UOW;
using UMS.Infrastructure.Repositories;
using UMS.Infrastructure.UOW;
using UMS.Persistence.Context;

namespace UMS.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>();
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        
        return services;
    }
}