var builder = WebApplication.CreateBuilder(args);

// Dodaj usługi do kontenera
builder.Services.AddOpenApi();

// Dodaj ręczne przekierowanie do HTTPS, jeśli aplikacja nie jest w trybie deweloperskim
builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 7253;  // Ustawienie portu HTTPS (upewnij się, że jest to taki port, jak w `launchSettings.json`)
});

var app = builder.Build();

// Konfiguracja środowiska
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // Mapowanie OpenAPI
}

// Wymuszanie przekierowania na HTTPS
app.UseHttpsRedirection();

app.UseRouting();

// Twoje mapowanie routingu
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

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

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
