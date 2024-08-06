using Microsoft.EntityFrameworkCore;
using NtaraFootballTest.Api.Entities;

namespace NtaraFootballTest.Api.Database;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<FootballTeam> FootballTeams { get; set; }
}
