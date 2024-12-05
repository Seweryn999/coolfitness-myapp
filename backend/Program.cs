using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using CoolFitnessBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// Ustawienie portu, na którym działa aplikacja
builder.WebHost.UseUrls("http://localhost:5000");

// Dodanie kontrolerów do serwera
builder.Services.AddControllers();

// Dodanie Swaggera (dokumentacja API)
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

// Dodanie generatora planów
builder.Services.AddSingleton<PlanGenerator>();

// Konfiguracja CORS (dopuszczenie połączeń z frontendu)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policyBuilder =>
        policyBuilder.WithOrigins("http://localhost:5173")
                     .AllowAnyHeader()
                     .AllowAnyMethod()
                     .AllowCredentials());
});

var app = builder.Build();

// Włączenie Swaggera tylko w środowisku deweloperskim
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoolFitness API v1");
    });
}

app.UseCors("AllowFrontend"); // Włączenie polityki CORS
app.UseAuthorization();

// Mapowanie kontrolerów
app.MapControllers();

app.Run();
