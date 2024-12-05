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
        public List<Exercise> Exercises { get; set; }
    }

    public class Exercise
    {
        public string Name { get; set; }
        public string Reps { get; set; }
        public string Sets { get; set; }
        public string Duration { get; set; }
    }

    public class WorkoutPlan
    {
        public string ExperienceLevel { get; set; }
        public string MuscleGroup { get; set; }
        public List<WorkoutDay> Plan { get; set; }
    }

    public class WorkoutPlansWrapper
    {
        public List<WorkoutPlan> Plans { get; set; }
    }
}
