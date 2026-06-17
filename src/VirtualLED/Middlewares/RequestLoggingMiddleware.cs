namespace VirtualLED.Middlewares;

/// <summary>
/// Source: https://blog.elmah.io/asp-net-core-request-logging-middleware/
/// </summary>
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _next = next;
        _logger = loggerFactory.CreateLogger<RequestLoggingMiddleware>();
    }

    public async Task Invoke(HttpContext context)
    {
        _logger.LogInformation(
            "Request {method} {url} => {statusCode}",
            context.Request?.Method,
            context.Request?.Path.Value,
            context.Response?.StatusCode);
        await _next(context);
    }
}
