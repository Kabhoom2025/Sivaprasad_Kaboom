using Event___Task_Scheduler_for_Remote_Teams.Models;
using Event___Task_Scheduler_for_Remote_Teams.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Event___Task_Scheduler_for_Remote_Teams.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;
        public TeamsController(ITeamService teamService)
        {
            _teamService = teamService;
        }
        [HttpGet("GetAllTeams")]
        public async Task<IActionResult> GetAllTeams()
        {
            var teams = await _teamService.GetAllTeamsAsync();
            return Ok(teams);
        }
        [HttpGet("GetTeam/{id}")]
        public async Task<IActionResult> GetTeamById(Guid id)
        {
            var team = await _teamService.GetTeamByIdAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            return Ok(team);
        }
        [HttpPost("CreateTeam")]
        public async Task<IActionResult> CreateTeam([FromBody] Team team)
        {
            if (team == null)
            {
                return BadRequest();
            }
            var createdTeam = await _teamService.CreateTeamAsync(team);
            return CreatedAtAction(nameof(GetTeamById), new { id = createdTeam.Id }, createdTeam);
        }
        [HttpDelete("Deleteteam/{id}")]
        public async Task<IActionResult> DeleteTeam(Guid id)
        {
            var result = await _teamService.DeleteTeamAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
