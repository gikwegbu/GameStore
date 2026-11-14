namespace GameStore.API.DTOs;
using System.ComponentModel.DataAnnotations;
public record  CreateGameDto(
    [Required][StringLength(50)][MinLength(3)] 
    string Name,
    // [StringLength(20)][MinLength(3)] 
    [Range(1,50)]
    int GenreId,
    [Range(0, 100)] 
    decimal Price,
    DateTime ReleaseDate
);
