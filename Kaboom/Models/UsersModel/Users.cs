using Kaboom.Models.Order;

namespace Kaboom.Models.Users
{
    public class Users
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string  PasswordHash { get; set; } // Hashed Password
        public string Role { get; set; }   // "User" or "Admin"
        public string ProfileImageUrl { get; set; } //User profile Image
       // public List<Orders> orders { get; set; } = new List<Orders>();
    }
}
