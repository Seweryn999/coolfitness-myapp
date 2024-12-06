using System.Collections.Generic;
using CoolFitnessBackend.Models;

namespace CoolFitnessBackend.Services
{
    public class PlanGenerator
    {
        public WorkoutPlansWrapper GeneratePlan(UserPreferences preferences)
        {
            // Na podstawie preferencji generujemy przykładowy plan
            var workoutPlan = new WorkoutPlan
            {
                ExperienceLevel = preferences.Intensity, // np. "Intermediate"
                MuscleGroup = preferences.Goal,         // np. "Legs"
                Plan = new List<WorkoutDay>
                {
                    new WorkoutDay
                    {
                        Day = 1,
                        Exercises = new List<Exercise>
                        {
                            new Exercise { Name = "Squat", Reps = "12-15", Sets = "4", Duration = "N/A" },
                            new Exercise { Name = "Leg Press", Reps = "10-12", Sets = "3", Duration = "N/A" }
                        }
                    },
                    new WorkoutDay
                    {
                        Day = 2,
                        Exercises = new List<Exercise>
                        {
                            new Exercise { Name = "Lunges", Reps = "10-12", Sets = "3", Duration = "N/A" },
                            new Exercise { Name = "Deadlift", Reps = "8-10", Sets = "3", Duration = "N/A" }
                        }
                    }
                }
            };

            return new WorkoutPlansWrapper { Plans = new List<WorkoutPlan> { workoutPlan } };
        }
    }

    // Definicje pomocnicze
    public class WorkoutDay
    {
        public int Day { get; set; }
        public required List<Exercise> Exercises { get; set; } = new();
    }

    public class Exercise
    {
        public required string Name { get; set; } = string.Empty;
        public required string Reps { get; set; } = string.Empty;
        public required string Sets { get; set; } = string.Empty;
        public required string Duration { get; set; } = string.Empty;
    }

    public class WorkoutPlan
    {
        public required string ExperienceLevel { get; set; } = string.Empty;
        public required string MuscleGroup { get; set; } = string.Empty;
        public required List<WorkoutDay> Plan { get; set; } = new();
    }

    public class WorkoutPlansWrapper
    {
        public required List<WorkoutPlan> Plans { get; set; } = new();
    }
}
