using System.Diagnostics;
using GameStore.Api.Authorization;
using GameStore.Api.Data;
using GameStore.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Configure dependency injection for the application
builder.Services.AddRepositories(builder.Configuration);

// add authentication and authorization
builder.Services.AddAuthentication().AddJwtBearer();

// Add authorization policies
builder.Services.AddGameAuthorization();

// Add HTTP logging services
builder.Services.AddHttpLogging(options =>
{
  // Configure the options here
});

var app = builder.Build();

// add a middleware to the pipeline to handle exceptions
app.Use(async (context, next) =>
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

    app.Logger.LogInformation(
      "{RequestMethod} {RequestPath} request took {ElapsedMilliseconds}ms to complete",
      context.Request.Method, context.Request.Path, elapsedMillisecond
    );
  }

});

// Automatically migrate the database
await app.Services.InitializeDbAsync();

// http logging
app.UseHttpLogging();

// Map the game endpoints for the application
app.MapGameEndPoints();



// Start the application
app.Run();



