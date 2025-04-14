using System.Security.Claims;
using Kaboom.Interfaces;
using Kaboom.Models.AuthUserModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kaboom.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authservices;
        public AuthController(IAuthService authservices)
        {
            _authservices = authservices;
        }
        [HttpGet("Allusers")]
        public IActionResult GetAllUsers()
        {
            var users = _authservices.GetAllUser();
            if (users == null || users.Count == 0)
            {
                return NotFound(new { Message = "No users found." });
            }
            // Optionally, you can return a custom message if no users are found
            return Ok(users);

        }
        [HttpGet("GetUser/{id}")]
        public IActionResult GetUserDetails(string id)
        {
            return Ok();
        }
        [HttpPost("register")]
        public IActionResult Register([FromBody] AuthUser authuser, [FromQuery] string password)
        {
            if (authuser == null || string.IsNullOrWhiteSpace(password))
            {
                return BadRequest("Invalid User Data");
            }
            var result = _authservices.RegisterUser(authuser, password);
            return Ok(new { message = "Registered successfully", user = result });
        }


        [Authorize(Roles = "Admin")]
        [HttpPatch("update-auth/{id}")]
        public IActionResult UpdateAuthUser(int id, [FromBody] AuthUser updateUser)
        {
            try
            {
                var authUser = _authservices.UpdateAuthUser(id, updateUser);
                return Ok(new { Message = "User Updated Successfully", authUser });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Invalid credentials");
            }
            try
            {
                var user = _authservices.GetUserByEmail(request.Email);
                if (user == null)
                {
                    return Unauthorized(new { Message = "Invalid credentials" });
                }
                var token = _authservices.Login(request.Email, request.Password);
                var refreshToken = _authservices.GenerateRefreshToken(user.Id);
                return Ok(new { Token = token, RefreshToken = refreshToken, User = user });

            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }
        [HttpPost("Refresh-token")]
        public IActionResult ValidaterefreshToken([FromBody] RefreshTokenRequest request)
        {
            var users = _authservices.GetUserByEmail(request.Email);
            if (users == null)
            {
                return Unauthorized(new { Message = "Invalid credentials" });
            }
            var isValid = _authservices.ValidateRefreshToken(users.Id, request.RefreshToken);
            if (!isValid)
            {
                return Unauthorized(new { Message = "Inavalid or expired Token" });
            }
            var user = new AuthUser
            {
                Id = users.Id,
                Email = request.Email,
                Role = users.Role,
                PasswordHash = users.PasswordHash,
                ProfileImageUrl = users.ProfileImageUrl,

            };
            var newJwtToken = _authservices.GenerateJWTtoken(user);
            return Ok(new { status = "Valid" });
        }
        [HttpPost("logout")]
        public IActionResult Logout([FromBody] LogOutRequest request)
        {
            if (request.UserId <= 0)
            {
                return BadRequest(new { message = "Invalid UsedId" });
            }
            var result = _authservices.Logout(request.UserId);
            return Ok(result);
        }
        [HttpDelete("Delete")]
        public IActionResult DeleteUsers([FromBody] List<int> userIds)
        {
            if (userIds == null || userIds.Count == 0)
            {
                return BadRequest(new { message = "No user IDs not Provided" });
            }
            var result = _authservices.DeleteAuthUsers(userIds);
            if (!result)
            {
                return NotFound(new { message = "some users are not found or already users deleted" });
            }
            return Ok(result + "Users Deleted Successfully");
        }
        [HttpGet("Validate-token")]
        public IActionResult ValidateToken()
        {
            return Ok(new { status = "Valid" });
        }


        [HttpPost("upload-profile-image")]
        public IActionResult UploadProfileImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest("No image uploaded.");
            }

            var imageName = Guid.NewGuid() + Path.GetExtension(image.FileName);
            var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", imageName);

            Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);

            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                image.CopyTo(stream);
            }

            var imageUrl = $"/images/{imageName}";
            return Ok(new { imageUrl });
        }
        [Authorize]
        [HttpGet("profile")]
        public IActionResult GetUserProfile()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized(new { Message = "Invalid Token" });
            }
            var user = _authservices.GetUserByEmail(userEmail);
            if (user == null)
                return NotFound(new { Message = "User not Found" });

            return Ok(user);
        }
    }
}