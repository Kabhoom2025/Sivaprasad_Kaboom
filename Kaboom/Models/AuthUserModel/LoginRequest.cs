namespace Kaboom.Models.AuthUserModel
{
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int UserId { get; set; } // Optional, can be fetched from DB

    }

}
