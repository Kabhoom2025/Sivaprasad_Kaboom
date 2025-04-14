using Kaboom.Models.AuthUserModel;

namespace Kaboom.Interfaces
{
    public interface IAuthService
    {
        List<AuthUser> GetAllUser();
        AuthUser RegisterUser(AuthUser user, string password);
        string GenerateJWTtoken(AuthUser user);
        string GenerateRefreshToken(int userId);
        bool ValidateRefreshToken(int UserId, string token);
        string Login(string email, string password);
        AuthUser GetUserByEmail(string email);
        string Logout(int id);
        public AuthUser UpdateAuthUser(int userId, AuthUser updatedAuthUser);
        public bool DeleteAuthUsers(List<int> userIds);
    }
}
