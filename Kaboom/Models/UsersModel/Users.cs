using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Kaboom.Models.Admin;

namespace Kaboom.Models.Users
{
    public class Users
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string PasswordHash { get; set; } // Hashed Password
        public string Role { get; set; }   // "User" or "Admin"
        public string ProfileImageUrl { get; set; } //User profile Image
        [NotMapped]
        public IFormFile ImageFile { get; set; } // For uploading image
        public int AdminId { get; set; }
        [JsonIgnore]
        public Admins? Admin { get; set; }
    }
}
