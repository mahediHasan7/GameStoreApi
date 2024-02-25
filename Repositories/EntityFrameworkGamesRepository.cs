using GameStore.Api.Data;
using GameStore.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Repositories;

public class EntityFrameworkGamesRepository : IGameRepository
{

    private readonly GameStoreContext dbContext;

    // The logger field is typically initialized via dependency injection in the constructor of the EntityFrameworkGamesRepository class. Once initialized, it can be used to create log messages at various levels (e.g., Information, Warning, Error) throughout the class.
    private readonly ILogger<EntityFrameworkGamesRepository> logger;

    // Constructor
    public EntityFrameworkGamesRepository(
        GameStoreContext dbContext,
        ILogger<EntityFrameworkGamesRepository> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    // Get
    public async Task<IEnumerable<Game>> GetAllAsync(int pageNumber, int pageSize)
    {
        var skipCount = (pageNumber - 1) * pageSize;
        return await dbContext.Games
                                    .OrderBy(game => game.Id)
                                    .Skip(skipCount)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();
    }

    // Get{id}
    public async Task<Game?> GetAsync(int id)
    {
        return await dbContext.Games.FindAsync(id);
    }

    // Post
    public async Task CreateAsync(Game game)
    {
        dbContext.Games.Add(game);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Game {GameId}:{GameName} was created", game.Id, game.Name);
    }

    // Put
    public async Task UpdateAsync(Game updatedGame)
    {
        dbContext.Games.Update(updatedGame);
        await dbContext.SaveChangesAsync();
    }

    // Delete
    public async Task DeleteAsync(int id)
    {
        await dbContext.Games.Where(game => game.Id == id)
                       .ExecuteDeleteAsync();
    }

    // Count total games
    public async Task<int> CountGamesAsync()
    {
        return await dbContext.Games.CountAsync();
    }
}