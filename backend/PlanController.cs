using Microsoft.AspNetCore.Mvc;
using CoolFitnessBackend.Services;
using CoolFitnessBackend.Models;

namespace CoolFitnessBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlanController : ControllerBase
    {
        private readonly PlanGenerator _planGenerator;

        public PlanController(PlanGenerator planGenerator)
        {
            _planGenerator = planGenerator ?? throw new ArgumentNullException(nameof(planGenerator));
        }

        [HttpPost("generate")]
        public IActionResult GeneratePlan([FromBody] UserPreferences preferences)
        {
            try
            {
                if (preferences == null ||
                    string.IsNullOrEmpty(preferences.Goal) ||
                    string.IsNullOrEmpty(preferences.Intensity) ||
                    preferences.Duration <= 0)
                {
                    Console.WriteLine("Invalid data received in request.");
                    return BadRequest(new { message = "Invalid data. Ensure all fields (Goal, Intensity, Duration) are provided and valid." });
                }

                Console.WriteLine($"Generating plan for preferences: Goal={preferences.Goal}, Intensity={preferences.Intensity}, Duration={preferences.Duration}");

                var plan = _planGenerator.GeneratePlan(preferences);

                if (plan == null)
                {
                    Console.WriteLine("Plan generation failed. No plan returned.");
                    return BadRequest(new { message = "Unable to generate a training plan for the provided preferences." });
                }

                return Ok(plan);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during plan generation: {ex}");
                return StatusCode(500, new { message = "An error occurred while generating the plan.", error = ex.Message });
            }
        }
    }
}