namespace CoolFitnessBackend.Models
{
    public class UserPreferences
    {
        public string Goal { get; set; } = string.Empty;
        public string Intensity { get; set; } = string.Empty;
        public int Duration { get; set; }
    }
}