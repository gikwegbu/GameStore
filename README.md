# GameStore API

A lightweight, RESTful Web API for managing a video game store catalog, built with **ASP.NET Core Minimal APIs (.NET 10)** and **Entity Framework Core (SQLite)**.

---

## Tutorial Reference

This project was built following the **ASP.NET Core Full Course For Beginners** tutorial on YouTube:

- [Watch Tutorial on YouTube](https://www.youtube.com/watch?v=YbRe4iIVYJk&t=4685s&pp=ygUnYXNwLm5ldCBjb3JlIGZ1bGwgY291cnNlIGZvciBiZWdpbmVlcnMg)

---

## Features

- **ASP.NET Core Minimal APIs**: Clean, fast endpoint routing grouped by resource (`/games` and `/genres`) using extension methods.
- **Entity Framework Core & SQLite**:
  - Persistent relational storage with `GameStoreContext`.
  - Automatic database migrations on application startup (`app.MigrateDb()`).
  - Automatic database seeding (`UseSeeding`) with default game genres (*Action*, *Adventure*, *Platformer*, *Sports*, *Strategy*, *RPG*).
  - Read-only query optimization with `.AsNoTracking()`.
- **DTO Pattern & Built-in Validation**:
  - Immutable `record` Data Transfer Objects (`CreateGameDto`, `UpdateGameDto`, `GameDetailsDto`, `GameSummaryDto`, `GenreDto`) to decouple internal database entities from the public API contract.
  - Request payload validation powered by `builder.Services.AddValidation()` and `System.ComponentModel.DataAnnotations` (`[Required]`, `[StringLength]`, `[MinLength]`, `[Range]`).
- **Ready-to-Use HTTP Requests**: Includes `games.http` for testing all CRUD endpoints directly from VS Code (REST Client extension) or Visual Studio.

---

## Tech Stack

- **Framework**: .NET 10.0 (`net10.0`) / ASP.NET Core Web API
- **ORM**: Entity Framework Core `10.0.14`
- **Database**: SQLite (`Microsoft.EntityFrameworkCore.Sqlite`)
- **Language**: C#

---

## Project Structure

```text
GameStore/
├── GameStore.API/
│   ├── Data/
│   │   ├── Migrations/              # EF Core database migrations
│   │   ├── DataExtensions.cs        # DB registration, seeding & auto-migration extensions
│   │   └── GameStoreContext.cs      # EF Core DbContext (Games & Genres)
│   ├── DTOs/
│   │   ├── CreateGameDto.cs         # Payload contract for creating a game
│   │   ├── UpdateGameDto.cs         # Payload contract for updating a game
│   │   ├── GameDetailsDto.cs        # Single game response DTO (with GenreId)
│   │   ├── GameSummaryDto.cs        # Game list item DTO (with Genre name)
│   │   └── GenreDto.cs              # Genre response DTO
│   ├── Endpoints/
│   │   ├── GamesEndpoints.cs        # CRUD endpoints for /games
│   │   └── GenreEndpoint.cs         # Read endpoint for /genres
│   ├── Models/
│   │   ├── Game.cs                  # Game domain entity
│   │   └── Genre.cs                 # Genre domain entity
│   ├── Properties/
│   │   └── launchSettings.json      # Local development launch profiles
│   ├── appsettings.json             # App configuration & SQLite connection string
│   ├── games.http                   # Sample HTTP requests for testing
│   ├── GameStore.API.csproj         # Project configuration & NuGet dependencies
│   └── Program.cs                   # Application entry point & middleware pipeline
└── README.md
```

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download) installed on your machine.
- (Optional) `dotnet-ef` CLI tool if you want to create or manage migrations manually:
  ```bash
  dotnet tool install --global dotnet-ef
  ```

### Installation & Running Locally

1. **Clone the repository**:
   ```bash
   git clone https://github.com/gikwegbu/GameStore.git
   cd GameStore
   ```

2. **Restore dependencies**:
   ```bash
   dotnet restore GameStore.API/GameStore.API.csproj
   ```

3. **Run the API**:
   ```bash
   dotnet run --project GameStore.API
   ```
   > On startup, the application automatically applies any pending EF Core migrations and seeds the `Genres` table if it is empty.

4. **Base URL**:
   - HTTP: `http://localhost:5296`
   - HTTPS: `https://localhost:7292`

---

## API Endpoints

### Games (`/games`)

| Method | Endpoint | Description | Request Body | Response |
| :--- | :--- | :--- | :--- | :--- |
| `GET` | `/games` | Get all games with genre names | *None* | `200 OK` (`List<GameSummaryDto>`) |
| `GET` | `/games/{id}` | Get a single game by ID | *None* | `200 OK` (`GameDetailsDto`) / `404 Not Found` |
| `POST` | `/games` | Create a new game | `CreateGameDto` | `201 Created` (`GameDetailsDto`) |
| `PUT` | `/games/{id}` | Update an existing game | `UpdateGameDto` | `204 No Content` / `404 Not Found` |
| `DELETE` | `/games/{id}` | Delete a game by ID | *None* | `204 No Content` / `404 Not Found` |

#### Sample `POST /games` Request

```http
POST http://localhost:5296/games
Content-Type: application/json

{
  "name": "The Legend of Zelda: Breath of the Wild",
  "genreId": 2,
  "price": 59.99,
  "releaseDate": "2017-03-03T00:00:00"
}
```

### Genres (`/genres`)

| Method | Endpoint | Description | Request Body | Response |
| :--- | :--- | :--- | :--- | :--- |
| `GET` | `/genres` | Get all available game genres | *None* | `200 OK` (`List<GenreDto>`) |

---

## Database & Migrations

The SQLite connection string is configured in `GameStore.API/appsettings.json`:

```json
"ConnectionStrings": {
  "GameStoreDb": "Data Source=GameStore.db"
}
```

To create a new migration after modifying the models:

```bash
cd GameStore.API
dotnet ef migrations add <MigrationName> --output-dir Data/Migrations
```
