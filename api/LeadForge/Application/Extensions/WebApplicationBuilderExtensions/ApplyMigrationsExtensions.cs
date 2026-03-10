using LeadForge.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LeadForge.Application.Extensions;

public static class ApplyMigrationsExtensions
{
    public static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var logger = scope.ServiceProvider
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger("Migration");

        try
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await db.Database.MigrateAsync();

            var seeder = new DatabaseSeeder(db);
            await seeder.SeedAsync();

            logger.LogInformation("Database migrations applied successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error applying database migrations.");
            throw;
        }
    }
}
