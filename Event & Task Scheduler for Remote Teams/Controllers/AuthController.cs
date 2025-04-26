using Event___Task_Scheduler_for_Remote_Teams.Models;
using Event___Task_Scheduler_for_Remote_Teams.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Event___Task_Scheduler_for_Remote_Teams.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthRequest request)
        {
            var result = await _authService.RegisterAsync(request);
            return result == null ? BadRequest("User already exists") : Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthRequest request)
        {
            var result = await _authService.LoginAsync(request);
            return result == null ? Unauthorized("Invalid credentials") : Ok(result);
        }
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var email = User.Identity?.Name;
            if (string.IsNullOrEmpty(email)) return Unauthorized();

            var result = await _authService.LogoutAsync(email);
            return result ? Ok(new { message = "Logged out successfully." }) : BadRequest("Logout failed.");
        }
        [Authorize]
        [HttpGet("secure-data")]
        public IActionResult GetSecureData()
        {
            return Ok("You are authorized!");
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshRequest request)
        {
            var result = await _authService.RefreshTokenAsync(request);
            return result == null ? Unauthorized("Invalid refresh token") : Ok(result);
        }

    }
}
