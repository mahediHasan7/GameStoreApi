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

// builder.Logging.AddJsonConsole(options =>
// {
//   options.JsonWriterOptions = new()
//   {
//     Indented = true
//   };
// });

// Add HTTP logging services
builder.Services.AddHttpLogging(options =>
{
  // Configure the options here
});

var app = builder.Build();

// Automatically migrate the database
await app.Services.InitializeDbAsync();

// http logging
app.UseHttpLogging();

// Map the game endpoints for the application
app.MapGameEndPoints();



// Start the application
app.Run();



