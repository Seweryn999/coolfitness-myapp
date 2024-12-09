using CoolFitnessBackend.Models;
using System.Text;  // For StringBuilder
using System.Text.Json;

namespace CoolFitnessBackend.Services
{
    public class PlanGenerator
    {
        private readonly string _jsonFilePath;
        private readonly List<WorkoutPlan> _workoutPlans;

        /// <summary>
        /// Constructor for PlanGenerator, initializes the workout plans from a JSON file.
        /// </summary>
        /// <param name="jsonFilePath">Path to the JSON file containing workout plans.</param>
        public PlanGenerator(string jsonFilePath)
        {
            if (string.IsNullOrWhiteSpace(jsonFilePath))
            {
                throw new ArgumentNullException(nameof(jsonFilePath), "The path to the JSON file cannot be null or empty.");
            }

            _jsonFilePath = jsonFilePath;

            // Load the JSON file containing workout plans.
            if (File.Exists(_jsonFilePath))
            {
                var jsonData = File.ReadAllText(_jsonFilePath);
                var workoutPlansWrapper = JsonSerializer.Deserialize<WorkoutPlansWrapper>(jsonData);
                _workoutPlans = workoutPlansWrapper?.Plans ?? new List<WorkoutPlan>();
            }
            else
            {
                throw new FileNotFoundException("Workout plan file not found.", _jsonFilePath);
            }
        }

        /// <summary>
        /// Generates a fitness plan based on user preferences.
        /// </summary>
        /// <param name="preferences">User preferences for fitness plan.</param>
        /// <returns>A generated FitnessPlan tailored to the user's preferences.</returns>
        public FitnessPlan GeneratePlan(UserPreferences preferences)
        {
            if (preferences == null)
            {
                throw new ArgumentNullException(nameof(preferences), "User preferences cannot be null.");
            }

            Console.WriteLine($"Generating plan for: Goal={preferences.Goal}, Intensity={preferences.Intensity}, Duration={preferences.Duration}");

            // Filter workout plans based on user preferences.
            var matchingPlans = _workoutPlans
                .Where(p => p.ExperienceLevel.Equals(preferences.Goal, StringComparison.OrdinalIgnoreCase))
                .Where(p => p.MuscleGroup.Equals(preferences.Intensity, StringComparison.OrdinalIgnoreCase))
                .ToList();

            // If no matching plans are found, return a default plan with a message.
            if (!matchingPlans.Any())
            {
                return new FitnessPlan
                {
                    Goal = preferences.Goal,
                    Intensity = preferences.Intensity,
                    Duration = preferences.Duration,
                    PlanDetails = "No matching workout plans found for your preferences. Please try again with different settings."
                };
            }

            // Select the first matching plan and generate details.
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

        /// <summary>
        /// Generates detailed information for a workout plan.
        /// </summary>
        /// <param name="workoutPlan">The workout plan to generate details for.</param>
        /// <returns>A string containing the detailed plan.</returns>
        private string GeneratePlanDetails(WorkoutPlan workoutPlan)
        {
            var details = new StringBuilder();

            // Iterate through each day in the plan.
            foreach (var day in workoutPlan.Plan)
            {
                details.AppendLine($"Day {day.Day}:");
                foreach (var exercise in day.Exercises)
                {
                    details.AppendLine($"- {exercise.Name}: {exercise.Sets} sets, {exercise.Reps} reps");
                }
                details.AppendLine(); // Add an empty line after each day.
            }

            return details.ToString();
        }
    }

    // Helper classes for deserialization of workout plans.

    public class WorkoutPlansWrapper
    {
        public List<WorkoutPlan> Plans { get; set; } = new List<WorkoutPlan>();
    }

    public class WorkoutPlan
    {
        public int Id { get; set; }
        public string MuscleGroup { get; set; } = string.Empty; // Muscle group focus
        public string ExperienceLevel { get; set; } = string.Empty; // Experience level (e.g., beginner, advanced)
        public List<DayPlan> Plan { get; set; } = new List<DayPlan>(); // Daily workout breakdown
    }

    public class DayPlan
    {
        public int Day { get; set; } // Day number
        public List<Exercise> Exercises { get; set; } = new List<Exercise>(); // Exercises for the day
    }

    public class Exercise
    {
        public string Name { get; set; } = string.Empty; // Name of the exercise
        public int Sets { get; set; } // Number of sets
        public int Reps { get; set; } // Number of repetitions
    }

    // Models used for generating fitness plans.

    public class FitnessPlan
    {
        public string Goal { get; set; } = string.Empty; // User's fitness goal
        public string Intensity { get; set; } = string.Empty; // Intensity of the workouts
        public int Duration { get; set; } // Duration of the plan in days
        public string PlanDetails { get; set; } = string.Empty; // Detailed description of the plan
    }

    public class UserPreferences
    {
        public string Goal { get; set; } = string.Empty; // Fitness goal (e.g., weight loss, muscle gain)
        public string Intensity { get; set; } = string.Empty; // Preferred workout intensity
        public int Duration { get; set; } // Duration of the plan in days
    }
}