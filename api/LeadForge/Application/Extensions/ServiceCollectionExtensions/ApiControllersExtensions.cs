using System.Text.Json.Serialization;

namespace LeadForge.Application.Extensions;

public static class ApiControllersExtensions
{
    public static IServiceCollection AddApiControllers(this IServiceCollection services)
    {
        services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters
                .Add(new JsonStringEnumConverter());
        });

    return services;
}
}