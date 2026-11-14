namespace GameStore.API.Endpoints;

using GameStore.API.Data;
using GameStore.API.DTOs;
using GameStore.API.Models;
using Microsoft.EntityFrameworkCore;

public static class GamesEndpoints {

 private static readonly List<GameSummaryDto> games = [
    new GameSummaryDto(1, "The Legend of Zelda: Breath of the Wild", "Action-Adventure", 59.99m, new DateTime(2017, 3, 3)),
    new (2, "Super Mario Odyssey", "Platformer", 49.99m, new DateTime(2017, 10, 27)),
    new GameSummaryDto(3, "Animal Crossing: New Horizons", "Simulation", 59.99m, new DateTime(2020, 3, 20))
 ];

 public static void MapGamesEndpoints(this WebApplication app) {
    const string GetGameById = "GetGameById";
    var gamesRoute = app.MapGroup("/games");
        
    // GET /games
    gamesRoute.MapGet("/", async (GameStoreContext dbContext) => await dbContext.Games
    .Include(game => game.Genre)
    .Select(game => new GameSummaryDto(
        game.Id,
        game.Name,
        game.Genre!.Name,
        game.Price,
        game.ReleaseDate
    ))
    .AsNoTracking() // Ensures the query is read-only and improves performance. Here we are telling EF Core that we don't intend to modify the retrieved entities.
    .ToListAsync());

    // Get single game by ID
    // app.MapGet("/games/{id}", (int id) => games.FirstOrDefault(g => g.Id == id)).WithName(GetGameById);
    gamesRoute.MapGet("/{id}", async (int id,  GameStoreContext dbContext) => {
        var game = await dbContext.Games.FindAsync(id);
        return game is not null ? Results.Ok(
            new GameDetailsDto(
                game.Id,
                game.Name,
                game.GenreId,
                game.Price,
                game.ReleaseDate
            )
        ) : Results.NotFound();
    }).WithName(GetGameById);

    // Create a new game
    gamesRoute.MapPost("/", async(CreateGameDto newGame, GameStoreContext dbContext) => {
        Game game = new () {
            Name = newGame.Name,
            GenreId = newGame.GenreId,
            Price = newGame.Price,
            ReleaseDate = newGame.ReleaseDate
        };
        dbContext.Games.Add(game);
        await dbContext.SaveChangesAsync();
        GameDetailsDto gameDetails = new (
            game.Id,
            game.Name,
            game.GenreId,
            game.Price,
            game.ReleaseDate
        );
        return Results.CreatedAtRoute(GetGameById, new { id = game.Id }, gameDetails);
    });

    gamesRoute.MapPut("/{id}", async(int id, UpdateGameDto updatedGame, GameStoreContext dbContext) => {
        /*
        var game = games.FirstOrDefault(g => g.Id == id);
        if (game is null) return Results.NotFound();

        game = game with {
            Name = updatedGame.Name ?? game.Name,
            Genre = updatedGame.Genre ?? game.Genre,
            Price = updatedGame.Price ?? game.Price,
            ReleaseDate = updatedGame.ReleaseDate ?? game.ReleaseDate
        };
        var index = games.FindIndex(g => g.Id == id);
        if (index != -1)
            games[index] = game;

        */
        var excitingGame = await dbContext.Games.FindAsync(id);
        if (excitingGame is null) return Results.NotFound();

        excitingGame.Name = updatedGame.Name;
        excitingGame.GenreId = updatedGame.GenreId;
        excitingGame.Price = updatedGame.Price;
        excitingGame.ReleaseDate = updatedGame.ReleaseDate; 

        await dbContext.SaveChangesAsync();

        return Results.NoContent();
    });

    gamesRoute.MapDelete("/{id}", async (int id, GameStoreContext dbContext) => {
        /* Shorter way: 
            await dbContext.Games.Where(g => g.Id == id).ExecuteDeleteAsync();
        */
        var game = await dbContext.Games.FindAsync(id);
        if (game is null) return Results.NotFound();
        dbContext.Games.Remove(game);
        await dbContext.SaveChangesAsync();
        return Results.NoContent();
    });
 }
}
