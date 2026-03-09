using LeadForge.Application.Interfaces;

namespace LeadForge.Application.Extensions;

public static class ApplicationServiceExtensions
{
   public static IServiceCollection AddApplicationServices(this IServiceCollection services)
   {
      services.AddHttpContextAccessor();
      services.AddScoped<IOpenAiService, OpenAiService>();
      services.AddScoped<IGenerationService, GenerationService>();
      services.AddScoped<ITokenService, TokenService>();
      services.AddScoped<IAuthService, AuthService>();
      services.AddScoped<ICurrentUserService, CurrentUserService>();
      services.AddScoped<IDashboardService, DashboardService>();

      return services;
   }
}