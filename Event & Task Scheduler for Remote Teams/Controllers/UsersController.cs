using System.Security.Claims;
using Event___Task_Scheduler_for_Remote_Teams.Models;
using Event___Task_Scheduler_for_Remote_Teams.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Event___Task_Scheduler_for_Remote_Teams.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        [HttpGet("GetUser/{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (user == null) return BadRequest();
            var createdUser = await _userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }
        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var authUserId = GetAuthUserId();
            var result = await _userService.GetProfileAsync(authUserId);
            return result == null ? NotFound() : Ok(result);
        }
        [HttpPost("profile")]
        public async Task<IActionResult> CreateProfile([FromBody] UserProfileRequest request)
        {
            var authUserId = GetAuthUserId();
            var result = await _userService.CreateProfileAsync(authUserId, request);
            return Ok(result);
        }
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserProfileRequest request)
        {
            var authUserId = GetAuthUserId();
            var result = await _userService.UpdateProfileAsync(authUserId, request);
            return result == null ? NotFound() : Ok(result);
        }

        private Guid GetAuthUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null ? Guid.Parse(claim.Value) : Guid.Empty;
        }
    }
}
