using CoolFitnessBackend.Models;

namespace CoolFitnessBackend.Services
{
    public class PlanGenerator
    {
        private readonly string _jsonFilePath;

        // Konstruktor PlanGenerator, który przyjmuje ścieżkę do pliku JSON
        public PlanGenerator(string jsonFilePath)
        {
            _jsonFilePath = jsonFilePath ?? throw new ArgumentNullException(nameof(jsonFilePath));  // Sprawdzamy null
        }

        // Metoda generująca plan treningowy na podstawie preferencji użytkownika
        public FitnessPlan GeneratePlan(UserPreferences preferences)
        {
            if (preferences == null) throw new ArgumentNullException(nameof(preferences));  // Sprawdzamy null

            return new FitnessPlan
            {
                Goal = preferences.Goal,
                Intensity = preferences.Intensity,
                Duration = preferences.Duration,
                PlanDetails = "Generated workout plan details here based on preferences."
            };
        }
    }
}
