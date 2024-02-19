using System.Diagnostics;
using GameStore.Api.Authorization;
using GameStore.Api.Dto;
using GameStore.Api.Entities;
using GameStore.Api.Repositories;

namespace GameStore.Api.Endpoints;

public static class GameEndpoints
{
  const string GetGameEndpointNameV1 = "GetGameV1";
  const string GetGameEndpointNameV2 = "GetGameV2";

  public static RouteGroupBuilder MapGameEndPoints(this IEndpointRouteBuilder endpoints)
  {

    var group = endpoints
                .NewVersionedApi()
                .MapGroup("/games")
                .HasApiVersion(1.0)
                .HasApiVersion(2.0)
                .WithParameterValidation();


    // V1 endpoints

    // GET
    group.MapGet("/", async (IGameRepository repository, ILoggerFactory loggerFactory) =>
    {
      return Results.Ok((await repository.GetAllAsync()).Select(game => game.AsDtoV1()));
    })
    .MapToApiVersion(1.0);

    // GET SINGLE
    group.MapGet("/{id}", async (IGameRepository repository, int id) =>
    {
      Game? game = await repository.GetAsync(id);
      if (game is null)
      {
        return Results.NotFound();
      }

      return Results.Ok(game.AsDtoV1());
    })
    .WithName(GetGameEndpointNameV1)
    .RequireAuthorization(Policies.ReadAccess)
    .MapToApiVersion(1.0);


    // V2 endpoints

    // GET
    group.MapGet("/", async (IGameRepository repository, ILoggerFactory loggerFactory) =>
    {
      return Results.Ok((await repository.GetAllAsync()).Select(game => game.AsDtoV2()));
    })
    .MapToApiVersion(2.0);

    // GET SINGLE
    group.MapGet("/{id}", async (IGameRepository repository, int id) =>
    {
      Game? game = await repository.GetAsync(id);
      if (game is null)
      {
        return Results.NotFound();
      }

      return Results.Ok(game.AsDtoV2());
    })
    .WithName(GetGameEndpointNameV2)
    .RequireAuthorization(Policies.ReadAccess)
    .MapToApiVersion(2.0);

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
      return Results.CreatedAtRoute(GetGameEndpointNameV1, new { id = game.Id }, game);
    })
    .RequireAuthorization(Policies.WriteAccess)
    .MapToApiVersion(1.0);

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
    })
    .RequireAuthorization(Policies.WriteAccess)
    .MapToApiVersion(1.0);

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
    })
    .RequireAuthorization(Policies.WriteAccess)
    .MapToApiVersion(1.0);

    return group;
  }
}