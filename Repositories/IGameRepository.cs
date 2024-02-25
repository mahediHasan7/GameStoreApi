using GameStore.Api.Entities;

namespace GameStore.Api.Repositories;

public interface IGameRepository
{
  Task<IEnumerable<Game>> GetAllAsync(int pageNumber, int pageSize);
  Task<Game?> GetAsync(int id);
  Task CreateAsync(Game game);
  Task UpdateAsync(Game updatedGame);
  Task DeleteAsync(int id);
  Task<int> CountGamesAsync();
}
