using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using CoolFitnessBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

// Register the PlanGenerator service as a singleton
builder.Services.AddSingleton<PlanGenerator>(provider =>
{
    var env = provider.GetRequiredService<IHostEnvironment>();
    var jsonFilePath = Path.Combine(env.ContentRootPath, "exercises.json");  // Path to your exercises data (if any)
    return new PlanGenerator(jsonFilePath);
});

// Add CORS policy to allow requests from the frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder => builder.WithOrigins("http://localhost:5173")  // Frontend URL (adjust if needed)
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

var app = builder.Build();

// Enable middleware to serve Swagger UI and generate Swagger docs in the development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoolFitness API v1");
    });
}

// Enable HTTPS redirection (optional)
app.UseHttpsRedirection();

// Enable CORS for frontend requests
app.UseCors("AllowFrontend");

// Enable serving static files (if you have any like images, JS, etc.)
app.UseDefaultFiles();
app.UseStaticFiles();

// Map fallback for Single Page Application (SPA) to serve the frontend (like React/Vue.js)
app.MapFallbackToFile("index.html");

// Endpoint to generate a workout plan based on user preferences
app.MapPost("/api/plan/generate", (UserPreferences preferences, PlanGenerator generator) =>
{
    var plan = generator.GeneratePlan(preferences);
    return Results.Json(plan);
})
.WithName("GeneratePlan")
.WithTags("Fitness Plan");

// Start the application
app.Run();

/// <summary>
/// WeatherForecast record for generating a mock weather forecast (optional endpoint)
/// </summary>
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

/// <summary>
/// User preferences for generating a workout plan
/// </summary>
public class UserPreferences
{
    public string Goal { get; set; } = string.Empty;  // User's fitness goal (e.g., Weight Loss, Muscle Gain)
    public string Intensity { get; set; } = string.Empty;  // Preferred intensity level (e.g., Low, Medium, High)
    public int Duration { get; set; }  // Duration in minutes or days for the plan
}
