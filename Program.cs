using Azure.Storage.Blobs;
using GameStore.Api.Authorization;
using GameStore.Api.Cors;
using GameStore.Api.Data;
using GameStore.Api.Endpoints;
using GameStore.Api.ErrorHandling;
using GameStore.Api.ImageUpload;
using GameStore.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Configure dependency injection for the application
builder.Services.AddRepositories(builder.Configuration);

// add authentication and authorization
builder.Services.AddAuthentication()
                .AddJwtBearer()
                .AddJwtBearer("Auth0");

// Add authorization policies
builder.Services.AddGameAuthorization();

// add versioning to the application (from nuget package Asp.Versioning.Http)
builder.Services.AddApiVersioning(options =>
{
  options.DefaultApiVersion = new(1.0);
  options.AssumeDefaultVersionWhenUnspecified = true;
});


// Configure CORS policy
builder.Services.ConfigureCors(builder.Configuration);

// Add HTTP logging services
builder.Services.AddHttpLogging(options =>
{
  // Configure the options here
});


// This code snippet is registering a singleton service for `IUploadImage` interface in the dependency
// injection container. It is creating a new instance of the `UploadImage` class and passing a new
// instance of `BlobContainerClient` as a parameter to the `UploadImage` constructor. The
// `BlobContainerClient` is initialized with the Azure Storage connection string retrieved from the
// configuration settings and the container name "images". This singleton service will be available
// throughout the application and can be injected into other classes that depend on `IUploadImage`.


builder.Services.AddSingleton<IUploadImage>(
  new UploadImage(
    new BlobContainerClient(
      builder.Configuration.GetConnectionString("AzureStorage"), "images"
    )
  )
);

builder.Logging.AddAzureWebAppDiagnostics();

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
app.MapImagesEndPoints();

app.UseCors();

// Start the application
app.Run();



