using FluentValidation;
using FluentValidation.AspNetCore;
using LeadForge.Api.Middleware;
using LeadForge.Application.Extensions;
using LeadForge.Application.Validators;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddLoggerConfiguration();

Log.Information("Starting LeadForge API...");

// --------------------
// Core
// --------------------

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// --------------------
// Swagger
// --------------------

builder.Services.AddSwaggerDocumentation();

// --------------------
// Validation
// --------------------

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<GeneratePostRequestValidator>();

// --------------------
// Application services
// --------------------

builder.Services.AddApplicationServices();
builder.Services.AddRateLimiterServices();

// --------------------
// Database
// --------------------

builder.Services.AddInfrastructure(builder.Configuration);

// --------------------
// Authentication
// --------------------

builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

// --------------------
// Middleware Pipeline
// --------------------

app.UseMiddleware<ExceptionHandlingMiddleware>();

// --------------------
// Development Tools
// --------------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LeadForge API v1");
        c.ConfigObject.AdditionalItems["persistAuthorization"] = "true";
    });
}

// --------------------
// Pipeline
// --------------------

app.UseSerilogRequestLogging();
app.UseRateLimiter();
// ---

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
