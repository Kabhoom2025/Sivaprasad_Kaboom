using Event___Task_Scheduler_for_Remote_Teams.Models;

namespace Event___Task_Scheduler_for_Remote_Teams.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(User user);
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(Guid id);
        Task<bool> DeleteUserAsync(Guid id);
        Task<UserProfileResponse?> GetProfileAsync(Guid authUserId);
        Task<UserProfileResponse> CreateProfileAsync(Guid authUserId, UserProfileRequest request);
        Task<UserProfileResponse?> UpdateProfileAsync(Guid authUserId, UserProfileRequest request);
    }
}
