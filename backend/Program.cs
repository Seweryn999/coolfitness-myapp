using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using CoolFitnessBackend.Services;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5000");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CoolFitness API",
        Version = "v1",
        Description = "API for generating personalized fitness plans."
    });
});

builder.Services.AddSingleton<PlanGenerator>(provider =>
{
    var env = provider.GetRequiredService<IHostEnvironment>();
    var jsonFilePath = Path.Combine(env.ContentRootPath, "exercises.json");

    if (!File.Exists(jsonFilePath))
    {
        Console.WriteLine($"File exercises.json not found at: {jsonFilePath}");
    }

    return new PlanGenerator(jsonFilePath);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policyBuilder =>
        policyBuilder.WithOrigins("http://localhost:5173")
                     .AllowAnyHeader()
                     .AllowAnyMethod()
                     .AllowCredentials());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoolFitness API v1");
    });
}

app.UseCors("AllowFrontend");

app.MapGet("/", () => "Welcome to the CoolFitness API!");

app.MapPost("/api/plan/generate", async (UserPreferences preferences, PlanGenerator generator) =>
{
    try
    {
        if (string.IsNullOrEmpty(preferences.Goal) || preferences.Duration <= 0)
        {
            return Results.Json(new { message = "Invalid data." }, statusCode: 400);
        }

        var plan = generator.GeneratePlan(preferences);

        return Results.Ok(plan);
    }
    catch (Exception ex)
    {
        return Results.Json(new { message = "Server error.", details = ex.Message }, statusCode: 500);
    }
})
.WithName("GeneratePlan")
.WithTags("Fitness Plan");

app.Run();

public class UserPreferences
{
    public string Goal { get; set; } = string.Empty;
    public string Intensity { get; set; } = string.Empty;
    public int Duration { get; set; }
}

public class FitnessPlan
{
    public string Goal { get; set; } = string.Empty;
    public string Intensity { get; set; } = string.Empty;
    public int Duration { get; set; }
    public string PlanDetails { get; set; } = string.Empty;
}