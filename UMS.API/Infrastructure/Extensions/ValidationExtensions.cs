using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace UMS.API.Infrastructure.Extensions;

public static class ValidationExtensions 
{
    public static IServiceCollection ConfigureValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

        return services;
    }
}