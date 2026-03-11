using LeadForge.Domain;
using LeadForge.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LeadForge.Application;

public class DatabaseSeeder
{
   private readonly AppDbContext _db;

    public DatabaseSeeder(AppDbContext db)
    {
        _db = db;
    }

    public async Task SeedAsync()
    {
        var demoUser = await _db.Users
            .FirstOrDefaultAsync(x => x.Email == "example@example.com");

        if (demoUser != null)
            return;

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "example@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("example123"),
            Credits = 1000,
            CreatedAt = DateTime.UtcNow
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

    }
}