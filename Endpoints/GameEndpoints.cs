using System.Diagnostics;
using GameStore.Api.Authorization;
using GameStore.Api.Dto;
using GameStore.Api.Entities;
using GameStore.Api.Repositories;

namespace GameStore.Api.Endpoints;

public static class GameEndpoints
{
  const string GetGameEndpointName = "GetGame";

  public static RouteGroupBuilder MapGameEndPoints(this IEndpointRouteBuilder endpoints)
  {

    var group = endpoints.MapGroup("/games").WithParameterValidation();

    // GET
    group.MapGet("/", async (IGameRepository repository, ILoggerFactory loggerFactory) =>
    {
      return Results.Ok((await repository.GetAllAsync()).Select(game => game.AsDto()));
    });

    // GET SINGLE
    group.MapGet("/{id}", async (IGameRepository repository, int id) =>
    {
      Game? game = await repository.GetAsync(id);
      if (game is null)
      {
        return Results.NotFound();
      }

      return Results.Ok(game.AsDto());
    })
    .WithName(GetGameEndpointName)
    .RequireAuthorization(Policies.ReadAccess);

    // POST
    group.MapPost("/", async (IGameRepository repository, CreateGameDto gameDto) =>
    {
      Game game = new()
      {
        Name = gameDto.Name,
        Genre = gameDto.Genre,
        Price = gameDto.Price,
        ReleaseDate = gameDto.ReleaseDate,
        ImageUri = gameDto.ImageUri
      };

      await repository.CreateAsync(game);
      return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
    }).RequireAuthorization(Policies.WriteAccess);

    // PUT
    group.MapPut("/{id}", async (IGameRepository repository, int id, UpdateGameDto gameDto) =>
    {

      Game? existingGame = await repository.GetAsync(id);
      if (existingGame is null)
      {
        return Results.NotFound();
      }

      existingGame.Name = gameDto.Name ?? existingGame.Name;
      existingGame.Genre = gameDto.Genre ?? existingGame.Genre;
      existingGame.Price = gameDto.Price != 0 ? gameDto.Price : existingGame.Price;
      existingGame.ReleaseDate = gameDto.ReleaseDate == default ? existingGame.ReleaseDate : gameDto.ReleaseDate;
      existingGame.ImageUri = gameDto.ImageUri ?? existingGame.ImageUri;

      await repository.UpdateAsync(existingGame);

      return Results.Ok(existingGame);
    }).RequireAuthorization(Policies.WriteAccess);

    // DELETE
    group.MapDelete("/{id}", async (IGameRepository repository, int id) =>
    {

      Game? game = await repository.GetAsync(id);
      if (game is null)
      {
        return Results.NotFound();
      }

      await repository.DeleteAsync(id);
      return Results.NoContent();
    }).RequireAuthorization(Policies.WriteAccess);

    return group;
  }
}