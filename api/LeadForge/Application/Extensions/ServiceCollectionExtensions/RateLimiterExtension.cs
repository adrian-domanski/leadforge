using Microsoft.AspNetCore.RateLimiting;

namespace LeadForge.Application.Extensions;

public static class RateLimiterExtension
{
   public static IServiceCollection AddRateLimiterServices(this IServiceCollection services)
   {
      services.AddRateLimiter(options =>
      {
         options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

         options.AddFixedWindowLimiter("generation", limiterOptions =>
         {
            limiterOptions.PermitLimit = 5;
            limiterOptions.Window = TimeSpan.FromMinutes(1);
            limiterOptions.QueueLimit = 0;
         });

         options.OnRejected = async (context, token) =>
         {
            context.HttpContext.Response.ContentType = "application/json";

            var response = new
            {
               message = "Too many generation requests. Please wait a moment.",
               statusCode = 429
            };

            await context.HttpContext.Response.WriteAsJsonAsync(response, token);
         };
      });


      return services;
   }
}