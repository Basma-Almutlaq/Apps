using System.Diagnostics;
public class RequestTimingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestTimingMiddleware> _logger;
    
    public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        await _next(context); 

        stopwatch.Stop();
        var elapsedMs = stopwatch.ElapsedMilliseconds;

        _logger.LogInformation($"Request to {context.Request.Path} took {elapsedMs} ms.");
    }
}