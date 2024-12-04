using CoolFitnessBackend.Models;
using System.Text;  // For StringBuilder
using System.Text.Json;

namespace CoolFitnessBackend.Services
{
    public class PlanGenerator
    {
        private readonly string _jsonFilePath;
        private readonly List<WorkoutPlan> _workoutPlans;

        // Constructor to initialize PlanGenerator with a JSON file path
        public PlanGenerator(string jsonFilePath)
        {
            _jsonFilePath = jsonFilePath ?? throw new ArgumentNullException(nameof(jsonFilePath));  // Null check

            // Load and deserialize data from the JSON file
            if (File.Exists(_jsonFilePath))
            {
                var jsonData = File.ReadAllText(_jsonFilePath);
                var workoutPlans = JsonSerializer.Deserialize<WorkoutPlansWrapper>(jsonData);
                _workoutPlans = workoutPlans?.Plans ?? new List<WorkoutPlan>();
            }
            else
            {
                throw new FileNotFoundException("The workout plan file was not found.", _jsonFilePath);
            }
        }

        // Method to generate a fitness plan based on user preferences
        public FitnessPlan GeneratePlan(UserPreferences preferences)
        {
            if (preferences == null) throw new ArgumentNullException(nameof(preferences));  // Null check

            // Find matching plans based on preferences
            var matchingPlans = _workoutPlans
                .Where(p => !string.IsNullOrEmpty(p.ExperienceLevel) &&
                            p.ExperienceLevel.Equals(preferences.Goal, StringComparison.OrdinalIgnoreCase))
                .Where(p => !string.IsNullOrEmpty(p.MuscleGroup) &&
                            p.MuscleGroup.Equals(preferences.Intensity, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!matchingPlans.Any())
            {
                throw new InvalidOperationException("No matching workout plans found for the given preferences.");
            }

            // Select a plan based on available data
            var selectedPlan = matchingPlans.FirstOrDefault();  // Additional criteria can be added here

            if (selectedPlan == null)
            {
                throw new InvalidOperationException("Failed to select a workout plan.");
            }

            // Generate plan details
            var planDetails = GeneratePlanDetails(selectedPlan);

            return new FitnessPlan
            {
                Goal = preferences.Goal ?? "Default Goal",
                Intensity = preferences.Intensity ?? "Default Intensity",
                Duration = preferences.Duration,
                PlanDetails = planDetails
            };
        }

        // Generate detailed plan information
        private string GeneratePlanDetails(WorkoutPlan workoutPlan)
        {
            if (workoutPlan == null) throw new ArgumentNullException(nameof(workoutPlan));  // Null check

            var details = new StringBuilder();

            foreach (var day in workoutPlan.Plan)
            {
                details.AppendLine($"Day {day.Day}:");
                foreach (var exercise in day.Exercises)
                {
                    details.AppendLine($"- {exercise.Name}: {exercise.Sets} sets, {exercise.Reps} reps");
                }
                details.AppendLine();
            }

            return details.ToString();
        }
    }

    // Helper classes for deserialization

    public class WorkoutPlansWrapper
    {
        public List<WorkoutPlan> Plans { get; set; } = new List<WorkoutPlan>();
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
