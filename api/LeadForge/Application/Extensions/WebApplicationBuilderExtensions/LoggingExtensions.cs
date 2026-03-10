using Serilog;

namespace LeadForge.Application.Extensions;

public static class LoggingExtensions
{
   public static void AddLoggerConfiguration(this WebApplicationBuilder builder)
   {
       builder.Host.UseSerilog();

       Log.Logger = new LoggerConfiguration()
           .Enrich.FromLogContext()
           .Enrich.WithThreadId()
           .Enrich.WithMachineName()
           .WriteTo.Console()
           .WriteTo.File(
               "logs/log-.txt",
               rollingInterval: RollingInterval.Day)
           .CreateLogger();

   }
}