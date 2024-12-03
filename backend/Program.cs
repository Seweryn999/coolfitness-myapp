using Microsoft.OpenApi.Models;
using CoolFitnessBackend.Services; 

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CoolFitness API",
        Version = "v1"
    });
});


builder.Services.AddSingleton<PlanGenerator>(provider =>
{
    var env = provider.GetRequiredService<IHostEnvironment>();
    var jsonFilePath = Path.Combine(env.ContentRootPath, "exercises.json");
    return new PlanGenerator(jsonFilePath);
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

app.UseHttpsRedirection();


app.UseDefaultFiles();  
app.UseStaticFiles();   


app.MapFallbackToFile("index.html");


app.MapGet("/weatherforecast", () =>
{
    var summaries = new[] 
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

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


app.MapPost("/generatePlan", (UserPreferences preferences, PlanGenerator generator) =>
{
    var plan = generator.GeneratePlan(preferences);
    return Results.Json(plan);
})
.WithName("GeneratePlan");

app.Run();


record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556); 
}


public class UserPreferences
{
    public string Goal { get; set; } = string.Empty; 
    public string Intensity { get; set; } = string.Empty; 
    public int Duration { get; set; }
}
