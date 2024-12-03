namespace CoolFitnessBackend.Services
{
    public class PlanGenerator
    {
        private readonly string _jsonFilePath;

        public PlanGenerator(string jsonFilePath)
        {
            _jsonFilePath = jsonFilePath;
        }

        public WorkoutPlan GeneratePlan(UserPreferences preferences)
        {
            // Generate and return the workout plan
            return new WorkoutPlan
            {
                Goal = preferences.Goal,
                Intensity = preferences.Intensity,
                Duration = preferences.Duration,
                PlanDetails = "Generated workout plan details here"
            };
        }
    }

    public class WorkoutPlan
    {
        public string Goal { get; set; }
        public string Intensity { get; set; }
        public int Duration { get; set; }
        public string PlanDetails { get; set; }
    }
}
