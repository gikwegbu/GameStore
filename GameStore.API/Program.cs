using GameStore.API;
using GameStore.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container for validatinig request data.
builder.Services.AddValidation();
builder.AddGameStoreDb();

var app = builder.Build();


// Games endpoints
app.MapGamesEndpoints();
// Genres endpoints
app.MapGenresEndpoint();

app.MigrateDb();


app.Run();
