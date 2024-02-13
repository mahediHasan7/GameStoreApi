using GameStore.Api.Entities;

namespace GameStore.Api.Repositories;

public class InMemGameRepository : IGameRepository
{
  private readonly List<Game> games =
    [
        new Game(){
            Id = 1,
            Name = "The Witcher 3: Wild Hunt",
            Genre = "Action RPG",
            Price = 29.99M,
            ReleaseDate = new DateTime(2015, 5, 19),
            ImageUri = "https://placeholder.co/100"
        },
        new Game(){
            Id = 2,
            Name = "Grand Theft Auto V",
            Genre = "Action-adventure",
            Price = 19.99M,
            ReleaseDate = new DateTime(2013, 9, 17),
            ImageUri = "https://placeholder.co/100"
        },
        new Game(){
            Id = 3,
            Name = "Red Dead Redemption 2",
            Genre = "Action-adventure",
            Price = 39.99M,
            ReleaseDate = new DateTime(2018, 10, 26),
            ImageUri = "https://placeholder.co/100"
        },
    ];

  public async Task<IEnumerable<Game>> GetAllAsync()
  {
    return await Task.FromResult(games);
  }

  public async Task<Game?> GetAsync(int id)
  {
    return await Task.FromResult(games.Find(game => game.Id == id));
  }


  public async Task CreateAsync(Game game)
  {
    int lastGameId = games.Count != 0 ? games.Max(game => game.Id) : 0;
    game.Id = lastGameId + 1;
    games.Add(game);

    await Task.CompletedTask;
  }

  public async Task UpdateAsync(Game updatedGame)
  {
    int index = games.FindIndex(existingGame => existingGame.Id == updatedGame.Id);
    games[index] = updatedGame;

    await Task.CompletedTask;
  }

  public async Task DeleteAsync(int id)
  {
    int index = games.FindIndex(game => game.Id == id);
    games.RemoveAt(index);

    await Task.CompletedTask;
  }

}