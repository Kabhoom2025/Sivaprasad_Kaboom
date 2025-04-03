namespace Kaboom.Models.AuthUserModel
{
    public class RefreshTokenRequest
    {
        public int UserId { get; set; }
        public string RefreshToken { get; set; }
        public string Email { get; set; } // Optional
    }
}
