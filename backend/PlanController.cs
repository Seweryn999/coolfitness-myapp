using Microsoft.AspNetCore.Mvc;
using CoolFitnessBackend.Services;

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
            Console.WriteLine($"Received preferences: Goal={preferences.Goal}, Intensity={preferences.Intensity}, Duration={preferences.Duration}");

            if (preferences == null ||
                string.IsNullOrEmpty(preferences.Goal) ||
                string.IsNullOrEmpty(preferences.Intensity) ||
                preferences.Duration <= 0)
            {
                return BadRequest(new { message = "Invalid data. Ensure all fields (Goal, Intensity, Duration) are provided and valid." });
            }

            try
            {
                var plan = _planGenerator.GeneratePlan(preferences);

                if (plan == null)
                {
                    return BadRequest(new { message = "Unable to generate a training plan for the provided preferences." });
                }

                return Ok(plan);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating plan: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while generating the plan." });
            }
        }
    }
}
