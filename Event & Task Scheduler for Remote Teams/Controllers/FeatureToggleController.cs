using Event___Task_Scheduler_for_Remote_Teams.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Event___Task_Scheduler_for_Remote_Teams.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class FeatureToggleController : ControllerBase
    {
        private readonly IFeatureToggleService _toggleService;

        public FeatureToggleController(IFeatureToggleService toggleService)
        {
            _toggleService = toggleService;
        }
        [HttpGet("GetAllFeatureKeys")]
        public async Task<IActionResult> GetAllToggles()
        {
            var toggles = await _toggleService.GetAllTogglesAsync();
            return Ok(toggles);
        }
        [HttpGet("{featureKey}")]
        public async Task<IActionResult> GetToggle(string featureKey)
        {
            var toggle = await _toggleService.GetToggleAsync(featureKey);
            if (toggle == null)
            {
                return NotFound();
            }
            return Ok(toggle);
        }
        [HttpPost("{featureKey}")]
        public async Task<IActionResult> UpdateToggle(string featureKey, [FromBody] bool isEnabled)
        {
            var toggle = await _toggleService.UpdateToggleAsync(featureKey, isEnabled);
            return Ok(toggle);
        }
        //[HttpDelete("{featureKey}")]
        //public async Task<IActionResult> DeleteToggle(string featureKey)
        //{
        //    var toggle = await _toggleService.GetToggleAsync(featureKey);
        //    if (toggle == null)
        //    {
        //        return NotFound();
        //    }
        //    _toggleService.DeleteToggle(toggle);
        //    return NoContent();
        //}
    }
}
