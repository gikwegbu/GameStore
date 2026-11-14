namespace GameStore.API.DTOs;

// A DTO is a contract between the API and the client, 
// defining the data structure that will be exchanged.

public record GameDetailsDto(
    int Id,
    string Name,
    int GenreId,
    decimal Price,
    DateTime ReleaseDate
);