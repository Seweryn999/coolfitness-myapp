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

        public PlanController()
        {
            _planGenerator = new PlanGenerator("path-to-config.json");
        }

        [HttpPost("generate")]
        public IActionResult GeneratePlan([FromBody] UserPreferences preferences)
        {
            if (preferences == null)
            {
                return BadRequest("Preferences cannot be null");
            }

            var plan = _planGenerator.GeneratePlan(preferences);
            return Ok(plan);
        }
    }
}
