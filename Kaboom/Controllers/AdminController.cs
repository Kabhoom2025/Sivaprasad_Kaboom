using Kaboom.Interfaces;
using Kaboom.Models;
using Kaboom.Models.Admin;
using Kaboom.Models.AuthUserModel;
using Kaboom.Models.Features;
using Kaboom.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace Kaboom.Controllers
{
    //  [Authorize(Roles ="Admin")]
    // [Route("api/[controller]")]
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IDataBaseService _dataBaseService;
        private readonly IAdminService _adminService;

        public AdminController(IDataBaseService dataBaseService, IAdminService adminService)
        {
            _dataBaseService = dataBaseService;
            _adminService = adminService;
        }
        [HttpPost("switch")]
        public IActionResult SwitchDatabases([FromBody] string provider)
        {
            if (provider != DatabaseProviders.Sqlserver && provider != DatabaseProviders.MongoDb)
            {
                return BadRequest("Invalid database Provider");
            }
            _dataBaseService.SetDatabaseProvider(provider);
            return Ok($"Database switched to {provider}");
        }
        [HttpGet("current")]
        public IActionResult GetCurrentDataBase()
        {
            return Ok(new { CurrentDatabase = _dataBaseService.GetCurrentDatabaseProvider() });
        }
        [HttpPost("register")]
        public IActionResult RegisterAdmin([FromBody] Admins admins)
        {
            try
            {
                var newAdmin = _adminService.RegisterAdmin(admins);
                return Ok(newAdmin);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetAdminById(int id)
        {
            var admin = _adminService.GetAdminById(id);
            if (admin == null)
                return NotFound("Admin not Found");
            return Ok(admin);
        }
        [HttpGet("Email/{email}")]
        public IActionResult GetAdminByEmail(string email)
        {
            var admin = _adminService.GetAdminByEmail(email);
            if (admin == null)
                return NotFound("Admin not Found");
            return Ok(admin);
        }
        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            var data = _adminService.GetAllUsers();
            var admin = _adminService.GetAllAdmins();
            var response = new
            {
                Users = data,
                Admins = admin
            };
            return Ok(response);
        }
        [HttpGet("products")]
        public IActionResult GetAllProducts()
        {
            var data = _adminService.GetAllProducts();
            return Ok(data);
        }
        [HttpGet("orders")]
        public IActionResult GetAllOrders()
        {
            var data = _adminService.GetAllOrders();
            return Ok(data);
        }
        [HttpPut("update/{id}")]
        public IActionResult UpdateAdminProfile(int id, [FromBody] Admins admins)
        {
            bool updated = _adminService.UpdateAdminProfile(id, admins);
            if (!updated)
                return NotFound("Admin not Found");
            return Ok("Admin profile Updated");
        }
        [HttpDelete("delete-user/{id}")]
        public IActionResult DeleteUser(int id)
        {
            bool deleted = _adminService.DeleteUser(id);
            if (!deleted)
                return NotFound("User not Found");
            return Ok("User Deleted Successfully");
        }
        //[Authorize(Roles = "Admin")]
        [HttpPost("register-user")]
        public IActionResult RegisterUser([FromBody] Users user)
        {
            try
            {
                var newUser = _adminService.RegisterUserFromAdmin(user);
                return Ok(newUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("set-feature")]
        public IActionResult SetFeature([FromBody] PreferenceToggle preferences)
        {
           var data = _adminService.SetFeature(preferences);
            if (data == null)
                return NotFound("Feature not Found");
            return Ok(data);
        }
        [HttpGet("get-feature")]
        public IActionResult GetFeature()
        {
            var data = _adminService.GetFeature();
            if (data == null)
                return NotFound("Feature not Found");
            return Ok(data);
        }
        [HttpGet("{adminId}/users")]
        public IActionResult GetUsersForAdmin(int adminId)
        {
            var data = _adminService.GetUsersForAdmin(adminId);
            if (data == null)
                return NotFound("No Users Found under this Admin");
            return Ok(data);
        }
        [HttpPost("{adminId}/register-user")]
        public IActionResult RegisterUserFromAdmin(int adminId, [FromBody] Users user)
        {
            if(user == null)
            {
                return BadRequest("User data is null");
            }
            var createuser = _adminService.RegisterUserFromAdmin(user, adminId);
            return CreatedAtAction(nameof(RegisterUserFromAdmin), new { id = createuser.AdminId }, createuser);
        }

    }
   
    
}
