using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Entities;

public class Game
{
  public int Id { get; set; }

  [Required]
  [StringLength(100)]
  public required string Name { get; set; }

  [Required]
  [StringLength(30)]
  public required string Genre { get; set; }

  [Range(0, 100)]
  public decimal Price { get; set; }

  public DateTime ReleaseDate { get; set; }

  [Required]
  [Url]
  public required string ImageUri { get; set; }
}