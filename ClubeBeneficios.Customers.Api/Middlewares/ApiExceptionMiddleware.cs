using System.Net;
using System.Text.Json;

namespace ClubeBeneficios.Customers.Api.Middlewares;

public sealed class ApiExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ApiExceptionMiddleware> _logger;

    public ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (UnauthorizedAccessException exception)
        {
            await WriteErrorAsync(context, HttpStatusCode.Unauthorized, exception.Message);
        }
        catch (KeyNotFoundException exception)
        {
            await WriteErrorAsync(context, HttpStatusCode.NotFound, exception.Message);
        }
        catch (InvalidOperationException exception)
        {
            await WriteErrorAsync(context, HttpStatusCode.BadRequest, exception.Message);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Erro nÃ£o tratado na requisiÃ§Ã£o do microserviÃ§o Customers.");
            await WriteErrorAsync(context, HttpStatusCode.InternalServerError, "Ocorreu um erro interno ao processar a requisiÃ§Ã£o.");
        }
    }

    private static async Task WriteErrorAsync(HttpContext context, HttpStatusCode statusCode, string message)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var payload = new
        {
            message
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }
}