using CoolFitnessBackend.Models;

namespace CoolFitnessBackend.Services
{
    public class PlanGenerator
    {
        private readonly string _jsonFilePath;

        // Constructor to inject the path to the JSON file containing exercises
        public PlanGenerator(string jsonFilePath)
        {
            _jsonFilePath = jsonFilePath;
        }

        // Method to generate a workout plan based on user preferences
        public WorkoutPlan GeneratePlan(UserPreferences preferences)
        {
            // You could enhance this method by reading the JSON file and using the data for plan generation.
            // This is a simple placeholder for the purpose of demonstration.

            return new WorkoutPlan
            {
                Goal = preferences.Goal,
                Intensity = preferences.Intensity,
                Duration = preferences.Duration,
                PlanDetails = "Generated workout plan details here based on preferences."
            };
        }
    }

    // Class representing a generated workout plan
    public class WorkoutPlan
    {
        public string Goal { get; set; } = string.Empty;  // Goal of the plan (e.g., Weight Loss, Muscle Gain)
        public string Intensity { get; set; } = string.Empty;  // Intensity level (e.g., Low, Medium, High)
        public int Duration { get; set; }  // Duration of the plan in minutes or days
        public string PlanDetails { get; set; } = string.Empty;  // Details of the generated plan
    }
}
