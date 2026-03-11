using LeadForge.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LeadForge.Application.Extensions;

public static class ApplyMigrationsExtensions
{
    public static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();

        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>()
            .CreateLogger("DatabaseMigration");

        const int maxRetries = 5;

        for (int i = 1; i <= maxRetries; i++)
        {
            try
            {
                logger.LogInformation("Applying database migrations...");
                await db.Database.MigrateAsync();
                logger.LogInformation("Database migrations applied successfully.");

                await seeder.SeedAsync();
                logger.LogInformation("Database seeding completed.");

                return;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Migration attempt {Attempt} failed", i);

                if (i == maxRetries)
                    throw;

                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }


    }
}
