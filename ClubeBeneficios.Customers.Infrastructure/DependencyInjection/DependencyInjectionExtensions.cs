using ClubeBeneficios.Customers.Domain.Interfaces;
using ClubeBeneficios.Customers.Domain.Repositories;
using ClubeBeneficios.Customers.Domain.Services;
using ClubeBeneficios.Customers.Infrastructure.Context;
using ClubeBeneficios.Customers.Infrastructure.Repositories;
using ClubeBeneficios.Customers.Infrastructure.Security;
using ClubeBeneficios.Customers.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ClubeBeneficios.Customers.Infrastructure.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddScoped<IUserContext, UserContext>();

        services.AddScoped<ICustomerAdminRepository, CustomerAdminRepository>();
        services.AddScoped<IPartnerCustomerAdminRepository, PartnerCustomerAdminRepository>();
        services.AddScoped<ICustomerPortalRepository, CustomerPortalRepository>();
        services.AddScoped<IPartnerCustomerPortalRepository, PartnerCustomerPortalRepository>();

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICustomerAdminService, CustomerAdminService>();
        services.AddScoped<IPartnerCustomerAdminService, PartnerCustomerAdminService>();
        services.AddScoped<ICustomerPortalService, CustomerPortalService>();
        services.AddScoped<IPartnerCustomerPortalService, PartnerCustomerPortalService>();

        return services;
    }
}