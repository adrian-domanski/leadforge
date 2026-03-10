using LeadForge.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LeadForge.Application.Extensions;

public static class ApplyMigrationsExtensions
{
    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        db.Database.Migrate();
    }
}
