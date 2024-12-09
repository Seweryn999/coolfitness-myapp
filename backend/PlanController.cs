using Microsoft.AspNetCore.Mvc;
using CoolFitnessBackend.Models;
using CoolFitnessBackend.Services;

namespace CoolFitnessBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlanController : ControllerBase
    {
        private readonly PlanGenerator _planGenerator;

        // Konstruktor PlanController, wstrzykuje zależność PlanGenerator
        public PlanController(PlanGenerator planGenerator)
        {
            _planGenerator = planGenerator ?? throw new ArgumentNullException(nameof(planGenerator));  // Sprawdzamy null
        }

        [HttpPost("generate")]
        public IActionResult GeneratePlan([FromBody] UserPreferences preferences)
        {
            Console.WriteLine($"Otrzymane dane: Goal={preferences.Goal}, Intensity={preferences.Intensity}, Duration={preferences.Duration}");

            if (preferences == null)
            {
                Console.WriteLine("Preferences cannot be null");
                return BadRequest("Preferences cannot be null");
            }

            var plan = _planGenerator.GeneratePlan(preferences);
            Console.WriteLine($"Wygenerowany plan: {plan.Goal}, {plan.Intensity}, {plan.Duration}");

            return Ok(plan);
        }
    }
}