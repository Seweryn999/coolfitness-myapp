namespace CoolFitnessBackend.Models
{
    public class UserPreferences
    {
        public required string Goal { get; set; }
        public required string Intensity { get; set; }
        public int Duration { get; set; }
    }
}