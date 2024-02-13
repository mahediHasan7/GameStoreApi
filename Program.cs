using GameStore.Api.Data;
using GameStore.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Configure dependency injection for the application
builder.Services.AddRepositories(builder.Configuration);

var app = builder.Build();

// Automatically migrate the database
await app.Services.InitializeDb();

// Map the game endpoints for the application
app.MapGameEndPoints();

// Start the application
app.Run();



