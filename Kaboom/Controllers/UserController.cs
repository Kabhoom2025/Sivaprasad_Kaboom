using System.Security.Claims;
using Kaboom.Interfaces;
using Kaboom.Models.UltimateModel;
using Kaboom.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kaboom.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody]Users user)
        {
            try
            {
                var newUser = _userService.Registeruser(user);
                if (newUser != null)
                {
                    return Ok(new { Message = "User Registered successfully",User = newUser });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            return Ok(user);
        }
       // [Authorize(Roles ="Admin")]
        [HttpPatch("update-user/{id}")]
        public IActionResult UpdateUser(int id, [FromBody]Users user)
        {
            try
            {
                var upuser = _userService.UpdateUser(id, user);
                return Ok(upuser);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            try
            {
                var user = _userService.GetUserById(id);
                return Ok(user);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("email/{email}")]
        public IActionResult GetUserByEmail(string email)
        {
            try
            {
                var user = _userService.GetUserByEmail(email);
                return Ok(user);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("Validate")]
        public IActionResult ValidateUser([FromBody] Users request)
        {
            try
            {
                var isValid = _userService.ValidateUser(request.UserEmail, request.PasswordHash);
                if (!isValid)
                    return Unauthorized(new { Message = "Invalid Email or Password" });
                return Ok(new {Message = "User Validated Successfully"});
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}/orders")]
        public IActionResult GetUserOrderHistory(int id)
        {
            try
            {
                var orders = _userService.GetUserOrderHistory(id);
                return Ok(orders);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("profile")]
        public IActionResult GetUserProfile()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if(string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized(new { Message = "Invalid Token" });
            }
            var user = _userService.GetUserByEmail(userEmail);
            if (user == null)
                return NotFound(new { Message = "User not Found" });

            return Ok(user);    
        }
       // [Authorize(Roles ="Admin")]
        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }
    }
}
