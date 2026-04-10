using ClubeBeneficios.Customers.Api.Middlewares;

namespace ClubeBeneficios.Customers.Api.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseApiExceptionHandling(this IApplicationBuilder app)
    {
        app.UseMiddleware<ApiExceptionMiddleware>();
        return app;
    }

    public static IApplicationBuilder UseUserContext(this IApplicationBuilder app)
    {
        app.UseMiddleware<UserContextMiddleware>();
        return app;
    }
}