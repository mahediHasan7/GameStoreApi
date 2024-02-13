using Microsoft.EntityFrameworkCore;
using GameStore.Api.Entities;
using System.Reflection;

namespace GameStore.Api.Data;

public class GameStoreContext : DbContext
{
  // DbContextOptions<GameStoreContext>: DbContextOptions is a class that carries configuration settings for a DbContext. GameStoreContext is the type argument. DbContextOptions<GameStoreContext> object contains the configuration settings for the DbContext, such as the database provider to use (e.g., SQL Server, SQLite, etc.) and the connection string.

  // base(options): This is calling the base class (DbContext) constructor with the options argument. This passes the configuration settings up to the DbContext, which knows how to use them to connect to the database.
  public GameStoreContext(DbContextOptions<GameStoreContext> options) : base(options)
  {
  }

  //Creates a DbSet that can be used to query and save instances of Game.
  public DbSet<Game> Games => Set<Game>();


  //in the OnModelCreating method is used to apply all configurations in the current assembly.

  // This means that Entity Framework Core will scan the assembly of the currently executing code for any classes that implement the IEntityTypeConfiguration interface and apply them.This is a convenient way to organize your configurations into separate classes and have them automatically applied, rather than manually applying each one.

  // If your GameConfigurations class is in the same assembly and it implements IEntityTypeConfiguration<Game>, it will be automatically picked up and applied by this line of code, even if it's not explicitly mentioned.

  // This approach helps to keep your DbContext clean and your configurations well-organized, especially when dealing with a large number of entity types.
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }
}