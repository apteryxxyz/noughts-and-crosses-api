using Microsoft.EntityFrameworkCore;
using NoughtsAndCrosses.Domain;

namespace NoughtsAndCrosses.Repository;

public class DatabaseContext : DbContext
{
    public DbSet<Game> Game { get; set; } = default!;

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseInMemoryDatabase("NoughtsAndCrosses");
}