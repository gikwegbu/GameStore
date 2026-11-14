namespace GameStore.API.DTOs;

// A DTO is a contract between the API and the client, 
// defining the data structure that will be exchanged.

public record GameSummaryDto(
    int Id,
    string Name,
    string Genre,
    decimal Price,
    DateTime ReleaseDate
);