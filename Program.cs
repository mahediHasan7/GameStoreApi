using GameStore.Api.Authorization;
using GameStore.Api.Data;
using GameStore.Api.Endpoints;
using GameStore.Api.ErrorHandling;
using GameStore.Api.Middleware;

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

// add built in exception handler middleware
app.UseExceptionHandler(exceptionHandlerApp => exceptionHandlerApp.ConfigureExceptionHandler());

// add a middleware to the pipeline to calculate the request timing
app.UseMiddleware<RequestTimingMiddleware>();

// Automatically migrate the database
await app.Services.InitializeDbAsync();

// http logging
app.UseHttpLogging();

// Map the game endpoints for the application
app.MapGameEndPoints();



// Start the application
app.Run();



