using GameStore.Api.Data;
using GameStore.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Repositories;

public class EntityFrameworkGamesRepository(GameStoreContext dbContext) : IGameRepository
{

    private readonly GameStoreContext dbContext = dbContext;

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