using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Dodaj usługi do kontenera.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CoolFitness API",
        Version = "v1"
    });
});

var app = builder.Build();

// Skonfiguruj potok żądań HTTP.
if (app.Environment.IsDevelopment())
{
    // Swagger jest dostępny tylko w trybie deweloperskim
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoolFitness API v1");
    });
}

app.UseHttpsRedirection();

// Obsługa plików statycznych (np. HTML, CSS, JS)
app.UseDefaultFiles();  // Używa domyślnego pliku (np. index.html)
app.UseStaticFiles();   // Umożliwia dostęp do plików statycznych (np. obrazy, CSS, JavaScript)

// Przekierowanie nieznanych ścieżek do aplikacji React (np. `index.html` w przypadku aplikacji SPA)
// Zapewnia to poprawne działanie aplikacji React po stronie klienta (SPA)
app.MapFallbackToFile("index.html");

// Endpoints API
var summaries = new[] 
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

// Endpoint zwracający prognozę pogody
app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

// Rekord prognozy pogody
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556); // Konwersja na stopnie Fahrenheita
}
