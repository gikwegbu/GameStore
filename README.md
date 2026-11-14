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
│   │   └── launchSettings.json      # Local development launch profiles (HTTP: 5296, HTTPS: 7292)
│   ├── appsettings.json             # App configuration & SQLite connection string
│   ├── games.http                   # Sample HTTP requests for testing
│   ├── GameStore.API.csproj         # Project configuration & NuGet dependencies
│   └── Program.cs                   # Application entry point & middleware pipeline
└── README.md
```

---

## Getting Started (Clone & Spin Up Locally)

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) installed on your machine (verify with `dotnet --list-sdks`).
- **VS Code** with the **REST Client** extension (for running requests in `GameStore.API/games.http`) and **C# Dev Kit** (optional, for debugging).
- (Optional) `dotnet-ef` CLI tool if you want to create or manage migrations manually:
  ```bash
  dotnet tool install --global dotnet-ef
  ```

### Step-by-Step Local Setup

1. **Clone the repository and navigate into the project folder**:
   ```bash
   git clone https://github.com/gikwegbu/GameStore.git
   cd GameStore
   ```

2. **Restore NuGet packages and build the project**:
   ```bash
   dotnet restore GameStore.API/GameStore.API.csproj
   dotnet build GameStore.API/GameStore.API.csproj
   ```

3. **(Optional) Trust the local ASP.NET Core HTTPS developer certificate**:
   ```bash
   dotnet dev-certs https --trust
   ```

4. **Start the API server**:
   - **Via Terminal (Recommended)**:
     ```bash
     dotnet run --project GameStore.API
     ```
   - **Via VS Code Run / Debug**:
     Press **`F5`** (or click **Run** in the C# Dev Kit sidebar). Both `http` and `https` launch profiles in `Properties/launchSettings.json` have `launchBrowser` set to `false` so the API starts quietly without opening a browser tab.

   > **Note**: On startup, the application automatically creates `GameStore.API/GameStore.db`, applies any pending EF Core migrations, and seeds the `Genres` table if it is empty.

5. **Verify the server is listening**:
   - **HTTP Base URL**: `http://localhost:5296`
   - **HTTPS Base URL**: `https://localhost:7292`

6. **Test the Endpoints**:
   Open `GameStore.API/games.http` in VS Code and click **Send Request** above any of the sample requests (`GET /games`, `POST /games`, `PUT /games/{id}`, `DELETE /games/{id}`, `GET /genres`).

7. **Stopping the Server**:
   - Press **`Ctrl + C`** in the terminal where `dotnet run` is active.
   - Or press **`Shift + F5`** if running via the VS Code debugger.
   - If a background process is still holding port `5296`, free it with:
     ```bash
     kill $(lsof -ti:5296)
     ```

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
  "name": "Samba",
  "genreId": 1,
  "price": 59.99,
  "releaseDate": "2027-06-01T00:00:00"
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

---

## Troubleshooting

- **`Framework 'Microsoft.NETCore.App', version '10.0.0' was not found`**:
  Install the [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0), or if you only have a newer major version installed (such as `.NET 11.0`), add `<RollForward>LatestMajor</RollForward>` inside `<PropertyGroup>` in `GameStore.API/GameStore.API.csproj`.
- **`DllNotFoundException: e_sqlite3` / `libe_sqlite3` on macOS (arm64)**:
  If the project folder was moved or copied from another directory, clean out stale `bin/` and `obj/` build caches and rebuild from scratch:
  ```bash
  rm -rf GameStore.API/bin GameStore.API/obj
  dotnet restore GameStore.API/GameStore.API.csproj
  dotnet build GameStore.API/GameStore.API.csproj
  ```
- **`Failed to bind to address http://127.0.0.1:5296: address already in use`**:
  1. Ensure no previous instance of `GameStore.API` is still running (`kill $(lsof -ti:5296)`).
  2. In `GameStore.API/Properties/launchSettings.json`, make sure the `"https"` profile uses distinct ports for HTTPS and HTTP (`"applicationUrl": "https://localhost:7292;http://localhost:5296"`).
