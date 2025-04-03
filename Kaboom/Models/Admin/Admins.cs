namespace Kaboom.Models.Admin
{
    public class Admins
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string? PasswordHash { get; set; } //Hashed Password
        public string ProfileImageUrl { get; set; } // Admin profile image
    }
}
