using Microsoft.Extensions.DependencyInjection;
using UMS.Application.Interfaces.Services;
using UMS.Application.Services;

namespace UMS.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}