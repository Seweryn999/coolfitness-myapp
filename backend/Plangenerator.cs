using CoolFitnessBackend.Models;
using System.Text.Json;

namespace CoolFitnessBackend.Services
{
    public class PlanGenerator
    {
        private readonly string _jsonFilePath;
        private readonly List<WorkoutPlan> _workoutPlans;

        // Konstruktor PlanGenerator, który przyjmuje ścieżkę do pliku JSON
        public PlanGenerator(string jsonFilePath)
        {
            _jsonFilePath = jsonFilePath ?? throw new ArgumentNullException(nameof(jsonFilePath));  // Sprawdzamy null

            // Wczytanie i deserializacja danych z pliku JSON
            if (File.Exists(_jsonFilePath))
            {
                var jsonData = File.ReadAllText(_jsonFilePath);
                var workoutPlans = JsonSerializer.Deserialize<WorkoutPlans>(jsonData);
                _workoutPlans = workoutPlans?.WorkoutPlans ?? new List<WorkoutPlan>();
            }
            else
            {
                throw new FileNotFoundException("Plik z ćwiczeniami nie został znaleziony.", _jsonFilePath);
            }
        }

        // Metoda generująca plan treningowy na podstawie preferencji użytkownika
        public FitnessPlan GeneratePlan(UserPreferences preferences)
        {
            if (preferences == null) throw new ArgumentNullException(nameof(preferences));  // Sprawdzamy null

            // Wyszukaj odpowiedni plan na podstawie preferencji (cel, poziom trudności, grupa mięśniowa)
            var matchingPlans = _workoutPlans
                .Where(p => p.ExperienceLevel.Equals(preferences.Goal, StringComparison.OrdinalIgnoreCase))
                .Where(p => p.MuscleGroup.Equals(preferences.Intensity, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!matchingPlans.Any())
            {
                throw new InvalidOperationException("Brak dopasowanych planów dla tych preferencji.");
            }

            // Wybór planu na podstawie dostępnych danych
            var selectedPlan = matchingPlans.FirstOrDefault();  // Możesz tu dodać logikę wybierania na podstawie innych kryteriów

            // Generowanie szczegółów planu
            var planDetails = GeneratePlanDetails(selectedPlan);

            return new FitnessPlan
            {
                Goal = preferences.Goal,
                Intensity = preferences.Intensity,
                Duration = preferences.Duration,
                PlanDetails = planDetails
            };
        }

        // Generowanie szczegółów planu na podstawie wybranego planu
        private string GeneratePlanDetails(WorkoutPlan workoutPlan)
        {
            var details = new StringBuilder();

            foreach (var day in workoutPlan.Plan)
            {
                details.AppendLine($"Dzień {day.Day}:");
                foreach (var exercise in day.Exercises)
                {
                    details.AppendLine($"- {exercise.Name}: {exercise.Sets} sets, {exercise.Reps} reps");
                }
                details.AppendLine();
            }

            return details.ToString();
        }
    }

    // Klasy pomocnicze do deserializacji JSON

    public class WorkoutPlans
    {
        public List<WorkoutPlan> WorkoutPlans { get; set; } = new List<WorkoutPlan>();
    }

    public class WorkoutPlan
    {
        public int Id { get; set; }
        public string MuscleGroup { get; set; } = string.Empty;
        public string ExperienceLevel { get; set; } = string.Empty;
        public List<DayPlan> Plan { get; set; } = new List<DayPlan>();
    }

    public class DayPlan
    {
        public int Day { get; set; }
        public List<Exercise> Exercises { get; set; } = new List<Exercise>();
    }

    public class Exercise
    {
        public string Name { get; set; } = string.Empty;
        public int Sets { get; set; }
        public int Reps { get; set; }
        public string Duration { get; set; } = string.Empty;
    }
}
