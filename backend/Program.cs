using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using CoolFitnessBackend.Services;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Ustawienie adresu URL, na którym aplikacja będzie dostępna (z HTTPS)
builder.WebHost.UseUrls("http://localhost:5000");

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
        policyBuilder.WithOrigins("http://localhost:5173") // Upewnij się, że frontend jest dostępny na tym porcie
                     .AllowAnyHeader()
                     .AllowAnyMethod()
                     .AllowCredentials());
});

var app = builder.Build();  // Tworzymy aplikację tylko raz

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
app.MapPost("/api/plan/generate", async (UserPreferences preferences) =>
{
    try
    {
        // Logowanie danych wejściowych (preferencji użytkownika)
        Console.WriteLine($"Otrzymane dane: Goal = {preferences.Goal}, Intensity = {preferences.Intensity}, Duration = {preferences.Duration}");

        // Walidacja danych wejściowych
        if (string.IsNullOrEmpty(preferences.Goal) || preferences.Duration <= 0)
        {
            return Results.Json(new { message = "Invalid data." }, statusCode: 400);
        }

        // Generowanie planu fitness
        var plan = new FitnessPlan
        {
            Goal = preferences.Goal,
            Intensity = preferences.Intensity,
            Duration = preferences.Duration,
            PlanDetails = "Some details here."
        };

        return Results.Ok(plan);
    }
    catch (Exception ex)
    {
        // Logowanie szczegółów błędu
        Console.WriteLine($"Wystąpił błąd: {ex.Message}");
        Console.WriteLine($"Szczegóły: {ex.StackTrace}");

        // Zwrócenie szczegółowego błędu w formacie JSON
        return Results.Json(new { message = "Server error.", details = ex.Message }, statusCode: 500);
    }
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
