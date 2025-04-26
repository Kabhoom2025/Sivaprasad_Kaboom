using Event___Task_Scheduler_for_Remote_Teams.Models;

namespace Event___Task_Scheduler_for_Remote_Teams.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthUser?> RegisterAsync(AuthRequest request);
        Task<AuthResponse?> LoginAsync(AuthRequest request);
        Task<AuthResponse?> RefreshTokenAsync(RefreshRequest request);
        Task<bool> LogoutAsync(string email);
    }
}
