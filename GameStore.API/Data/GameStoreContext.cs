using System;
using Microsoft.EntityFrameworkCore;
using GameStore.API.Models;
namespace GameStore.API.Data;

// public class GameStoreContext: DbContext {
    // public GameStoreContext(DbContextOptions<GameStoreContext> options) : base(options) { }
public class GameStoreContext(DbContextOptions<GameStoreContext> options): DbContext(options) { 

    public DbSet<Game> Games => Set<Game>();
    public DbSet<Genre> Genres => Set<Genre>();
}
