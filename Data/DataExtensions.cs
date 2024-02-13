
using GameStore.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;


public static class DataExtensions
{

  public static async Task InitializeDb(this IServiceProvider serviceProvider)
  {
    using var scope = serviceProvider.CreateScope();
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<GameStoreContext>();
    await dbContext.Database.MigrateAsync();
  }

  public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
  {
    // connection to the database
    var connectionString = configuration.GetConnectionString("GameStoreContext");

    // Register the GameStoreContext with the DI container
    services.AddSqlServer<GameStoreContext>(connectionString)
            .AddScoped<IGameRepository, EntityFrameworkGamesRepository>();

    return services;
  }

}