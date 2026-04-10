using System.Security.Claims;
using ClubeBeneficios.Customers.Domain.Interfaces;

namespace ClubeBeneficios.Customers.Api.Middlewares;

public sealed class UserContextMiddleware
{
    private readonly RequestDelegate _next;

    public UserContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IUserContext userContext)
    {
        if (context.User?.Identity?.IsAuthenticated == true)
        {
            userContext.UserId =
                context.User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                context.User.FindFirstValue("sub");

            userContext.Email =
                context.User.FindFirstValue(ClaimTypes.Email) ??
                context.User.FindFirstValue("email");

            userContext.Role =
                context.User.FindFirstValue(ClaimTypes.Role) ??
                context.User.FindFirstValue("role");

            userContext.SessionId = context.User.FindFirstValue("session_id");
            userContext.PartnerId = context.User.FindFirstValue("partner_id");
            userContext.Origin = context.User.FindFirstValue("origin");
        }

        await _next(context);
    }
}