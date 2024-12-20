using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using CoolFitnessBackend.Services;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Dodanie usług do kontenera
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

// Rejestracja PlanGenerator jako singleton
builder.Services.AddSingleton<PlanGenerator>(provider =>
{
    var env = provider.GetRequiredService<IHostEnvironment>();
    var jsonFilePath = Path.Combine(env.ContentRootPath, "exercises.json");

    // Sprawdzenie, czy plik istnieje
    if (!File.Exists(jsonFilePath))
    {
        Console.WriteLine($"Plik exercises.json nie został znaleziony w ścieżce: {jsonFilePath}");
    }

    return new PlanGenerator(jsonFilePath);
});

// Dodanie polityki CORS dla frontendowych żądań
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policyBuilder =>
        policyBuilder.WithOrigins("http://localhost:5173")
                     .AllowAnyHeader()
                     .AllowAnyMethod()
                     .AllowCredentials());
});

var app = builder.Build();

// Włączenie Swagger UI w środowisku deweloperskim
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoolFitness API v1");
    });
}

// Włączenie obsługi CORS
app.UseCors("AllowFrontend");

// Dodanie endpointu głównego / (root), który będzie działał na https://localhost:5000/
app.MapGet("/", () => "Witaj w aplikacji CoolFitness API!");

// Mapowanie endpointów API
app.MapPost("/api/plan/generate", (UserPreferences preferences, PlanGenerator generator) =>
{
    // Logowanie otrzymanych danych
    Console.WriteLine($"Otrzymane dane: {preferences.Goal}, {preferences.Intensity}, {preferences.Duration}");

    // Weryfikacja danych wejściowych
    if (string.IsNullOrEmpty(preferences.Goal) || preferences.Duration <= 0)
    {
        Console.WriteLine("Invalid user preferences provided.");
        return Results.BadRequest("Invalid user preferences provided.");
    }

    // Generowanie planu treningowego
    var plan = generator.GeneratePlan(preferences);
    Console.WriteLine($"Wygenerowany plan: {plan.Goal}, {plan.Intensity}, {plan.Duration}");
    return Results.Ok(plan);
})
.WithName("GeneratePlan")
.WithTags("Fitness Plan");

// Uruchomienie aplikacji
app.Run();

// Definicje klas dla użytkownika i planu
public class UserPreferences
{
    public string Goal { get; set; } = string.Empty;      // Added default value
    public string Intensity { get; set; } = string.Empty; // Added default value
    public int Duration { get; set; }
}

public class FitnessPlan
{
    public string Goal { get; set; } = string.Empty;         // Added default value
    public string Intensity { get; set; } = string.Empty;    // Added default value
    public int Duration { get; set; }
    public string PlanDetails { get; set; } = string.Empty;  // Added default value
}