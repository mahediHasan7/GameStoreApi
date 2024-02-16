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

var app = builder.Build();

// Automatically migrate the database
await app.Services.InitializeDb();

// Map the game endpoints for the application
app.MapGameEndPoints();

// Start the application
app.Run();



