using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GameStore.Api.Entities;

namespace GameStore.Api.Data.Configurations;

public class GameConfigurations : IEntityTypeConfiguration<Game>
{
  public void Configure(EntityTypeBuilder<Game> builder)
  {
    builder.Property(game => game.Price).HasPrecision(5, 2);
  }

}