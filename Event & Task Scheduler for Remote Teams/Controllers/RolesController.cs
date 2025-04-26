using Event___Task_Scheduler_for_Remote_Teams.Models;
using Event___Task_Scheduler_for_Remote_Teams.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Event___Task_Scheduler_for_Remote_Teams.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetRolesForUser(Guid userId)
        {
            var roles = await _roleService.GetRolesForUserAsync(userId);
            if (roles == null || !roles.Any())
                return NotFound("No roles found for this user.");
            return Ok(roles);
        }
        [HttpPost("{userId}")]
        public async Task<IActionResult> AddRole(Guid userId, [FromBody] string roleName)
        {
            var result = await _roleService.AddRoleToUserAsync(userId, roleName);
            return result ? Ok() : BadRequest("Failed to add role");
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> RemoveRole(Guid userId, [FromBody] string roleName)
        {
            var result = await _roleService.RemoveRoleFromUserAsync(userId, roleName);
            return result ? Ok() : BadRequest("Failed to remove role");
        }
        [HttpPost("assign-multiple/{userId}")]
        public async Task<IActionResult> AssignMultipleRolesToUser(Guid userId, [FromBody] List<Role> roleNames)
        {
            if (roleNames == null || !roleNames.Any())
            {
                return BadRequest("Role list cannot be empty.");
            }

            var result = await _roleService.AssignMultipleRolesToUserAsync(userId, roleNames);
            if (result)
                return Ok("Roles assigned successfully.");
            return BadRequest("Failed to assign roles.");
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            if (roles == null || !roles.Any())
                return NotFound("No roles found.");
            return Ok(roles);
        }
    }
}
