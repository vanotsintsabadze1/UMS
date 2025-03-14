using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using UMS.API.Models.Response;

namespace UMS.API.Infrastructure.Extensions;

public static class ValidationExtensions
{
    public static IServiceCollection ConfigureValidation(this IServiceCollection services)
    {
        services.AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(e => e.Value?.Errors.Count > 0)
                        .ToDictionary(
                            e => e.Key.ToLowerInvariant(),
                            e => e.Value!.Errors.Select(err => err.ErrorMessage)
                                .ToArray());

                    var response = new ApiResponse("The given data does not follow validation rules", errors);

                    return new BadRequestObjectResult(response);
                };
            });


        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

        return services;
    }

    public static IRuleBuilderOptions<T, string> MustBelongToSingleAlphabet<T>(this IRuleBuilder<T, string> ruleBuilder)
        where T : class
    {
        return ruleBuilder
            .Matches(@"^[a-zA-Z]+$|^[ა-ჰ]+$")
            .WithMessage("Name must contain only Latin or only Georgian letters.");
    }

    public static IRuleBuilderOptions<T, string> MustBeParsedIntoNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(val => long.TryParse(val, out _))
            .WithMessage((_, val) => $"{val} is not a valid number");
    }
}