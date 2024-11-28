
// DEMO

// using System;
// using System.IO;
// using System.Linq;
// using System.Net.Http;
// using System.Text.Json;
// using System.Threading.Tasks;
// using Firebase.Database;
// using Firebase.Database.Query;

// // Model for user data
// public class UserData
// {
//     public string Name { get; set; }
//     public string Email { get; set; }
//     public string PhoneNumber { get; set; }
//     public int Height { get; set; }
//     public int Weight { get; set; }
//     public int Age { get; set; }
//     public int GymExperience { get; set; }
//     public string MuscleGroups { get; set; }
// }

// // Model for the exercise plan
// public class ExercisePlan
// {
//     public int Id { get; set; }
//     public string MuscleGroup { get; set; }
//     public string ExperienceLevel { get; set; }
//     public PlanDay[] Plan { get; set; }
// }

// // Model for a day's workout plan
// public class PlanDay
// {
//     public int Day { get; set; }
//     public Exercise[] Exercises { get; set; }
// }

// // Model for a single exercise
// public class Exercise
// {
//     public string Name { get; set; }
//     public int Sets { get; set; }
//     public int? Reps { get; set; }
//     public string Duration { get; set; }
// }

// public class WorkoutPlanGenerator
// {
//     private const string FirebaseUrl = "https://coolfitness-f1486-default-rtdb.firebaseio.com/";
//     private const string JsonFilePath = "Exercises.json"; // Path to the JSON file containing workout plans

//     public static async Task Main(string[] args)
//     {
//         try
//         {
//             // Fetch user data from Firebase
//             var userData = await FetchUserDataFromFirebase();

//             if (userData == null)
//             {
//                 Console.WriteLine("No user data found.");
//                 return;
//             }

//             // Load the JSON file with workout plans
//             var exercisePlans = LoadExercisePlans();

//             // Generate a workout plan based on user data
//             var workoutPlan = GenerateWorkoutPlan(userData, exercisePlans);

//             if (workoutPlan != null)
//             {
//                 Console.WriteLine("Workout Plan Generated:");
//                 foreach (var day in workoutPlan.Plan)
//                 {
//                     Console.WriteLine($"Day {day.Day}:");
//                     foreach (var exercise in day.Exercises)
//                     {
//                         Console.WriteLine($"  - {exercise.Name}: {exercise.Sets} sets" +
//                                           $"{(exercise.Reps.HasValue ? $", {exercise.Reps} reps" : "")}" +
//                                           $"{(!string.IsNullOrEmpty(exercise.Duration) ? $", {exercise.Duration}" : "")}");
//                     }
//                 }
//             }
//             else
//             {
//                 Console.WriteLine("No suitable workout plan found.");
//             }
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine($"An error occurred: {ex.Message}");
//         }
//     }

//     /// <summary>
//     /// Fetches the most recent user data from Firebase.
//     /// </summary>
//     /// <returns>UserData object or null if no data is found.</returns>
//     private static async Task<UserData> FetchUserDataFromFirebase()
//     {
//         var client = new FirebaseClient(FirebaseUrl);
//         var entries = await client
//             .Child("Entries")
//             .OnceAsync<UserData>();

//         return entries.LastOrDefault()?.Object; // Fetch the last submitted entry
//     }

//     /// <summary>
//     /// Loads the exercise plans from the JSON file.
//     /// </summary>
//     /// <returns>An array of ExercisePlan objects.</returns>
//     private static ExercisePlan[] LoadExercisePlans()
//     {
//         var jsonData = File.ReadAllText(JsonFilePath);
//         return JsonSerializer.Deserialize<ExercisePlan[]>(jsonData, new JsonSerializerOptions
//         {
//             PropertyNameCaseInsensitive = true
//         });
//     }

//     /// <summary>
//     /// Matches the best workout plan based on the user's data.
//     /// </summary>
//     /// <param name="user">User data object.</param>
//     /// <param name="plans">Array of available workout plans.</param>
//     /// <returns>The best matching ExercisePlan or null if no match is found.</returns>
//     private static ExercisePlan GenerateWorkoutPlan(UserData user, ExercisePlan[] plans)
//     {
//         // Determine experience level based on gym experience in months
//         string experienceLevel = user.GymExperience switch
//         {
//             <= 6 => "beginner",
//             <= 24 => "intermediate",
//             _ => "advanced"
//         };

//         // Find a matching plan based on muscle group and experience level
//         return plans.FirstOrDefault(plan =>
//             string.Equals(plan.MuscleGroup, user.MuscleGroups, StringComparison.OrdinalIgnoreCase) &&
//             plan.ExperienceLevel == experienceLevel);
//     }
// }
