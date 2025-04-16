using System.Text.Json.Serialization;

namespace Kaboom.Models.AuthUserModel
{
    public class AuthUser
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Salt { get; set; }
        public string? Role { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public string? ProfileImageUrl { get; set; } // Profile image
        public string? PlainTextPassword { get; set; }
        public int AdminId { get; set; }
    }
    public class RefreshToken
    {
        public int Id { get; set; }
        public string? Token { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsRevoked { get; set; }
        public int AuthUserId { get; set; }
        [JsonIgnore]
        public AuthUser? AuthUser { get; set; }
    }
}
