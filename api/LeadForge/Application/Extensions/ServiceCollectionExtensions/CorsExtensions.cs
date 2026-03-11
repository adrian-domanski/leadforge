namespace LeadForge.Application.Extensions;

public static class CorsExtensions
{
   public static IServiceCollection AddCorsPolicyServices(this IServiceCollection services)
   {
      services.AddCors(options =>
      {
         options.AddPolicy("frontend", policy =>
         {
            policy
               .WithOrigins("http://localhost:3000", "https://leadforge.kodario.com/login",
               "https://www.leadforge.kodario.com/login")
               .AllowAnyHeader()
               .AllowAnyMethod();
         });
      });

      return services;
   }
}