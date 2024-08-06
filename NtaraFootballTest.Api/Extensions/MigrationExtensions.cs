using CsvHelper;
using Microsoft.EntityFrameworkCore;
using NtaraFootballTest.Api.Database;
using NtaraFootballTest.Api.Entities;
using System.Globalization;

namespace NtaraFootballTest.Api.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var dbContext = services.GetRequiredService<ApplicationDbContext>();
        var logger = services.GetRequiredService<ILogger<WebApplication>>();

        try
        {
            // Apply pending migrations
            dbContext.Database.Migrate();
            logger.LogInformation("Database migration applied successfully.");

            // Seed the database with initial data
            SeedDatabase(dbContext, app.Configuration, logger);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating or seeding the database.");
        }
    }

    private static void SeedDatabase(
        ApplicationDbContext dbContext,
        IConfiguration configuration,
        ILogger logger
    )
    {
        if (!dbContext.FootballTeams.Any())
        {
            var csvFilePath = configuration["CsvDataPath"];
            try
            {
                using var reader = new StreamReader(csvFilePath!);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                var records = csv.GetRecords<FootballTeam>().ToList();

                dbContext.FootballTeams.AddRange(records);
                dbContext.SaveChanges();
                logger.LogInformation("Database seeded successfully with CSV data.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }
        else
        {
            logger.LogInformation("Database already seeded, skipping seeding.");
        }
    }
}
