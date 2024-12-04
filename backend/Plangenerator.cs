using CoolFitnessBackend.Models;
using System.Text;  // For StringBuilder
using System.Text.Json;

namespace CoolFitnessBackend.Services
{
    public class PlanGenerator
    {
        private readonly string _jsonFilePath;
        private readonly List<WorkoutPlan> _workoutPlans;

        public PlanGenerator(string jsonFilePath)
        {
            _jsonFilePath = jsonFilePath ?? throw new ArgumentNullException(nameof(jsonFilePath));

            // Wczytujemy plik JSON z planami
            if (File.Exists(_jsonFilePath))
            {
                var jsonData = File.ReadAllText(_jsonFilePath);
                var workoutPlans = JsonSerializer.Deserialize<WorkoutPlansWrapper>(jsonData);
                _workoutPlans = workoutPlans?.Plans ?? new List<WorkoutPlan>();
            }
            else
            {
                throw new FileNotFoundException("Nie znaleziono pliku z planem treningowym.", _jsonFilePath);
            }
        }

        // Metoda generująca plan na podstawie preferencji użytkownika
        public FitnessPlan GeneratePlan(UserPreferences preferences)
        {
            Console.WriteLine($"Generowanie planu dla: Cel={preferences.Goal}, Intensywność={preferences.Intensity}, Czas trwania={preferences.Duration}");

            // Dopasowanie planów na podstawie celów i intensywności
            var matchingPlans = _workoutPlans
                .Where(p => p.ExperienceLevel.Equals(preferences.Goal, StringComparison.OrdinalIgnoreCase))
                .Where(p => p.MuscleGroup.Equals(preferences.Intensity, StringComparison.OrdinalIgnoreCase))
                .ToList();

            // Jeśli brak dopasowanych planów
            if (!matchingPlans.Any())
            {
                return new FitnessPlan
                {
                    Goal = preferences.Goal,
                    Intensity = preferences.Intensity,
                    Duration = preferences.Duration,
                    PlanDetails = "Nie znaleziono planów treningowych odpowiadających Twoim preferencjom. Spróbuj ponownie z innymi ustawieniami."
                };
            }

            // Wybór pierwszego dopasowanego planu
            var selectedPlan = matchingPlans.First();
            var planDetails = GeneratePlanDetails(selectedPlan);

            return new FitnessPlan
            {
                Goal = preferences.Goal,
                Intensity = preferences.Intensity,
                Duration = preferences.Duration,
                PlanDetails = planDetails
            };
        }

        // Metoda generująca szczegóły planu treningowego
        private string GeneratePlanDetails(WorkoutPlan workoutPlan)
        {
            var details = new StringBuilder();

            // Przechodzimy przez każdy dzień planu
            foreach (var day in workoutPlan.Plan)
            {
                details.AppendLine($"Dzień {day.Day}:");
                foreach (var exercise in day.Exercises)
                {
                    details.AppendLine($"- {exercise.Name}: {exercise.Sets} serie, {exercise.Reps} powtórzenia");
                }
                details.AppendLine(); // Pusta linia po każdym dniu
            }

            return details.ToString();
        }
    }

    // Klasy pomocnicze do deserializacji planów z JSON-a

    public class WorkoutPlansWrapper
    {
        public List<WorkoutPlan> Plans { get; set; } = new List<WorkoutPlan>();
    }

    public class WorkoutPlan
    {
        public int Id { get; set; }
        public string MuscleGroup { get; set; } = string.Empty; // Grupa mięśni
        public string ExperienceLevel { get; set; } = string.Empty; // Poziom doświadczenia (np. początkujący, zaawansowany)
        public List<DayPlan> Plan { get; set; } = new List<DayPlan>(); // Plan podzielony na dni
    }

    public class DayPlan
    {
        public int Day { get; set; } // Numer dnia
        public List<Exercise> Exercises { get; set; } = new List<Exercise>(); // Ćwiczenia w danym dniu
    }

    public class Exercise
    {
        public string Name { get; set; } = string.Empty; // Nazwa ćwiczenia
        public int Sets { get; set; } // Liczba serii
        public int Reps { get; set; } // Liczba powtórzeń
    }
}
