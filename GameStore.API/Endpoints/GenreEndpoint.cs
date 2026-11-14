using System;
using GameStore.API.Data;
using GameStore.API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Endpoints;

public static class GenreEndpoint {
    public static void MapGenresEndpoint(this WebApplication app) {
        var genresRoute = app.MapGroup("/genres");
        
        genresRoute.MapGet("/", async (GameStoreContext dbContext) => {
            var genres = await dbContext.Genres
            .Select(g => new GenreDto(g.Id, g.Name))
            .AsNoTracking()
            .ToListAsync();
            return Results.Ok(genres);
        });
    }
}
