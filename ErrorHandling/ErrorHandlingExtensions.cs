using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Api.ErrorHandling;

public static class ErrorHandlingExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {

        // the Run method is to add a terminal middleware in the pipeline. Terminal middleware handles the request and generates a response without passing control to any other middlewares.

        app.Run(async context =>
        {
            var logger = context.RequestServices.GetRequiredService<ILoggerFactory>()
                                .CreateLogger("Error Handling");

            var exceptionDetails = context.Features.Get<IExceptionHandlerFeature>();
            var exception = exceptionDetails?.Error;

            logger.LogError(exception, "An error occurred while getting all games on machine {Machine}. TraceId: {TraceId}", Environment.MachineName, Activity.Current?.TraceId);

            var problem = new ProblemDetails()
            {
                Title = "An error occurred while getting all games",
                Status = StatusCodes.Status500InternalServerError,
                Extensions = {
                    { "TraceId", Activity.Current?.TraceId.ToString()},
                    { "Machine", Environment.MachineName.ToString()}
                }
            };

            // only for development
            var environment = context.RequestServices.GetRequiredService<IHostEnvironment>();
            if (environment.IsDevelopment())
            {
                problem.Detail = exception?.ToString();
            }

            await Results.Problem(problem).ExecuteAsync(context);

        });
    }


}