using System.Collections.Generic;
using CoolFitnessBackend.Models;

namespace CoolFitnessBackend.Services
{
    public class PlanGenerator
    {
        public WorkoutPlansWrapper GeneratePlan(UserPreferences preferences)
        {
            // Generowanie przykładowego planu treningowego na podstawie preferencji użytkownika
            var workoutPlan = new WorkoutPlan
            {
                ExperienceLevel = preferences.Intensity, // np. "Intermediate"
                MuscleGroup = preferences.Goal,         // np. "Legs"
                Plan = GenerateWorkoutDays(preferences)
            };

            return new WorkoutPlansWrapper { Plans = new List<WorkoutPlan> { workoutPlan } };
        }

        private List<WorkoutDay> GenerateWorkoutDays(UserPreferences preferences)
        {
            var workoutDays = new List<WorkoutDay>();

            if (preferences.Goal.ToLower() == "legs")
            {
                workoutDays.Add(new WorkoutDay
                {
                    Workout = 1,
                    Exercises = new List<Exercise>
                    {
                        new Exercise { Name = "Bodyweight Squats", Reps = "15", Sets = "3", Duration = "N/A" },
                        new Exercise { Name = "Glute Bridge", Reps = "12", Sets = "3", Duration = "N/A" },
                        new Exercise { Name = "Calf Raises", Reps = "15", Sets = "3", Duration = "N/A" }
                    }
                });

                workoutDays.Add(new WorkoutDay
                {
                    Workout = 2,
                    Exercises = new List<Exercise>
                    {
                        new Exercise { Name = "Lunges", Reps = "12", Sets = "3", Duration = "N/A" },
                        new Exercise { Name = "Side Leg Raises", Reps = "15", Sets = "3", Duration = "N/A" },
                        new Exercise { Name = "Step-Ups", Reps = "12", Sets = "3", Duration = "N/A" }
                    }
                });
            }

            if (preferences.Intensity.ToLower() == "advanced")
            {
                workoutDays.Add(new WorkoutDay
                {
                    Workout = 3,
                    Exercises = new List<Exercise>
                    {
                        new Exercise { Name = "Deadlifts", Reps = "6", Sets = "4", Duration = "N/A" },
                        new Exercise { Name = "Power Cleans", Reps = "5", Sets = "3", Duration = "N/A" },
                        new Exercise { Name = "Barbell Squats", Reps = "6", Sets = "4", Duration = "N/A" }
                    }
                });
            }

            return workoutDays;
        }
    }

    public class WorkoutDay
    {
        public int Workout { get; set; }
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