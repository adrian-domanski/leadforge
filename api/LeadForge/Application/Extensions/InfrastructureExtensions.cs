using LeadForge.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LeadForge.Application.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection")
            ));

        return services;
    }
}