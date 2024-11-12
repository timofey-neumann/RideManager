using Domain.Exceptions;

namespace WEB.Middleware;

public class LoggerMiddleware(RequestDelegate next, ILogger<LoggerMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (BadRequestException ex)
        {
            logger.LogError(ex, "Ошибка валидации: {Message}", ex.Message);
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            await context.Response.WriteAsync($"Ошибка валидации: {ex.Message}");
        }
        catch (NotFoundException ex)
        {
            logger.LogError(ex, "Объект не найден: {Message}", ex.Message);
            context.Response.StatusCode = StatusCodes.Status404NotFound;

            await context.Response.WriteAsync($"Объект не найден: {ex.Message}");
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogError(ex, "Ошибка доступа: {Message}", ex.Message);
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            await context.Response.WriteAsync($"Ошибка доступа: {ex.Message}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Произошла ошибка при обработке запроса.");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsync("Произошла внутренняя ошибка сервера.");
        }
    }
}

