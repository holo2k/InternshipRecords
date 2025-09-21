using System.Diagnostics;
using System.Text;
using Serilog;
using ILogger = Serilog.ILogger;

namespace AccountService.Startup.Middleware;

public class RequestLoggingMiddleware
{
    private readonly ILogger _logger;
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
        _logger = Log.ForContext<RequestLoggingMiddleware>();
    }

    public async Task Invoke(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            var requestBody = string.Empty;
            if (context.Request.ContentLength > 0 &&
                (context.Request.Method == HttpMethods.Post || context.Request.Method == HttpMethods.Put))
            {
                context.Request.EnableBuffering();
                using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, false, leaveOpen: true);
                requestBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            _logger.Debug("Incoming HTTP Request {@Method} {@Path} {@QueryString} {@Headers} {@Body}",
                context.Request.Method,
                context.Request.Path,
                context.Request.QueryString.ToString(),
                context.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
                requestBody);

            await _next(context);

            stopwatch.Stop();

            _logger.Information("HTTP Response {@Method} {@Path} {@StatusCode} completed in {ElapsedMilliseconds}ms",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            _logger.Error(ex, "Unhandled exception for HTTP {Method} {Path} completed in {ElapsedMilliseconds}ms",
                context.Request.Method,
                context.Request.Path,
                stopwatch.ElapsedMilliseconds);

            throw;
        }
    }
}