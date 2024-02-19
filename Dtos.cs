using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dto;

public record GameDtoV1(
    int Id,
    string Name,
    string Genre,
    decimal Price,
    DateTime ReleaseDate,
    string ImageUri
);


public record GameDtoV2(
    int Id,
    string Name,
    string Genre,
    decimal Price,
    decimal RetailPrice,
    DateTime ReleaseDate,
    string ImageUri
);


public record CreateGameDto(
    [Required][StringLength(100)] string Name,
    [Required][StringLength(30)] string Genre,
    [Range(0, 100)] decimal Price,
    DateTime ReleaseDate,
    [Required][Url] string ImageUri
);

public record UpdateGameDto(
    [Required][StringLength(100)] string Name,
    [Required][StringLength(30)] string Genre,
    [Range(0, 100)] decimal Price,
    DateTime ReleaseDate,
    [Required][Url] string ImageUri
);