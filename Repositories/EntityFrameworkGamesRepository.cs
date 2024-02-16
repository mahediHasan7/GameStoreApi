using GameStore.Api.Data;
using GameStore.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Repositories;

public class EntityFrameworkGamesRepository : IGameRepository
{

    private readonly GameStoreContext dbContext;

    // The logger field is typically initialized via dependency injection in the constructor of the EntityFrameworkGamesRepository class. Once initialized, it can be used to create log messages at various levels (e.g., Information, Warning, Error) throughout the class.
    private readonly ILogger<EntityFrameworkGamesRepository> logger;

    public EntityFrameworkGamesRepository(
        GameStoreContext dbContext,
        ILogger<EntityFrameworkGamesRepository> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    public async Task<IEnumerable<Game>> GetAllAsync()
    {
        // ToListAsync() method is an Entity Framework extension method that asynchronously enumerates the query results and sends them to a list.
        return await dbContext.Games.AsNoTracking().ToListAsync();
    }

    public async Task<Game?> GetAsync(int id)
    {
        return await dbContext.Games.FindAsync(id);
    }
    public async Task CreateAsync(Game game)
    {
        dbContext.Games.Add(game);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Game {GameId}:{GameName} was created", game.Id, game.Name);
    }

    public async Task UpdateAsync(Game updatedGame)
    {
        dbContext.Games.Update(updatedGame);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await dbContext.Games.Where(game => game.Id == id)
                       .ExecuteDeleteAsync();
    }
}