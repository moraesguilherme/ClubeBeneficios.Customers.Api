using FluentValidation.AspNetCore;
using System.Text.Json;

namespace ClubeBeneficios.Customers.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiControllers(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            })
            .AddFluentValidation(configuration =>
            {
                configuration.RegisterValidatorsFromAssemblyContaining<Program>();
                configuration.DisableDataAnnotationsValidation = true;
            });

        services.AddEndpointsApiExplorer();

        return services;
    }
}