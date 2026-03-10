using FluentValidation;
using FluentValidation.AspNetCore;
using LeadForge.Api.Middleware;
using LeadForge.Application.Extensions;
using LeadForge.Application.Validators;
using LeadForge.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// --------------------
// Logging
// --------------------

builder.AddLoggerConfiguration();
Log.Information("Starting LeadForge API...");

// --------------------
// Services
// --------------------

builder.Services
    .AddApiControllers()
    .AddEndpointsApiExplorer()
    .AddSwaggerDocumentation()
    .AddFluentValidationAutoValidation()
    .AddValidatorsFromAssemblyContaining<GeneratePostRequestValidator>()
    .AddApplicationServices()
    .AddRateLimiterServices()
    .AddCorsPolicyServices()
    .AddInfrastructure(builder.Configuration)
    .AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

// --------------------
// Middleware
// --------------------

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "LeadForge API v1");
        options.ConfigObject.AdditionalItems["persistAuthorization"] = "true";
    });
}

app.UseSerilogRequestLogging();

app.UseCors("frontend");

app.UseRateLimiter();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health");

app.MapControllers();

// --------------------
// Database migration
// --------------------

app.ApplyMigrations();

app.Run();
