using LeadForge.Domain;
using Microsoft.EntityFrameworkCore;

namespace LeadForge.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Generation> Generations => Set<Generation>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Generation>()
            .HasOne(g => g.User)
            .WithMany(u => u.Generations)
            .HasForeignKey(g => g.UserId);
    }
}