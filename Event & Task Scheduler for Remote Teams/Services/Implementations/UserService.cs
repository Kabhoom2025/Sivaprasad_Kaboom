using Event___Task_Scheduler_for_Remote_Teams.Data;
using Event___Task_Scheduler_for_Remote_Teams.Models;
using Event___Task_Scheduler_for_Remote_Teams.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Event___Task_Scheduler_for_Remote_Teams.Services.Implementations
{
    public class UserService:IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserProfileResponse> CreateProfileAsync(Guid authUserId, UserProfileRequest request)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                AuthUserId = authUserId,
                UserData = request.UserData
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserProfileResponse
            {
                Id = user.Id,
                AuthUserId = user.AuthUserId,
                UserData = user.UserData
            };
        }

        public async Task<User> CreateUserAsync(User user)
        {
            user.Id = Guid.NewGuid();
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<UserProfileResponse?> GetProfileAsync(Guid authUserId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.AuthUserId == authUserId);
            if (user == null) return null;

            return new UserProfileResponse
            {
                Id = user.Id,
                AuthUserId = user.AuthUserId,
                UserData = user.UserData
            };
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<UserProfileResponse?> UpdateProfileAsync(Guid authUserId, UserProfileRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.AuthUserId == authUserId);
            if (user == null) return null;

            user.UserData = request.UserData;
            await _context.SaveChangesAsync();

            return new UserProfileResponse
            {
                Id = user.Id,
                AuthUserId = user.AuthUserId,
                UserData = user.UserData
            };
        }
    }
}
