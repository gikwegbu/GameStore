namespace GameStore.API.DTOs;
using System.ComponentModel.DataAnnotations;
public record UpdateGameDto (
    [Required][StringLength(50)][MinLength(3)] 
    string Name,
    [Range(1,50)]
    int GenreId,
    [Range(0, 100)] 
    decimal Price,
    DateTime ReleaseDate
);
