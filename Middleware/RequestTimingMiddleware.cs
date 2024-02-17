using System.Diagnostics;

namespace GameStore.Api.Middleware;

public class RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
{
    private readonly RequestDelegate next = next;
    private readonly ILogger<RequestTimingMiddleware> logger = logger;

    // has to be same as the following convention
    public async Task InvokeAsync(HttpContext context)
    {
        var stopWatch = new Stopwatch();
        try
        {
            stopWatch.Start();
            await next(context);
        }
        finally
        {
            stopWatch.Stop();
            var elapsedMillisecond = stopWatch.ElapsedMilliseconds;

            logger.LogInformation(
              "{RequestMethod} {RequestPath} request took {ElapsedMilliseconds}ms to complete",
              context.Request.Method, context.Request.Path, elapsedMillisecond
            );
        }
    }
}