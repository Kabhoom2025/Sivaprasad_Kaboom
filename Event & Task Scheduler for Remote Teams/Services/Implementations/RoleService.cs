using Event___Task_Scheduler_for_Remote_Teams.Data;
using Event___Task_Scheduler_for_Remote_Teams.Models;
using Event___Task_Scheduler_for_Remote_Teams.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Event___Task_Scheduler_for_Remote_Teams.Services.Implementations
{
    public class RoleService:IRoleService
    {
        private readonly ApplicationDbContext _context;


        public RoleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddRoleToUserAsync(Guid userId, string roleName)
        {
            var user = await _context.Users.FindAsync(userId);
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);

            if (user == null || role == null)
                return false;

            user.Roles.Add(role);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AssignMultipleRolesToUserAsync(Guid userId, List<Role> roleNames)
        {
            var user = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return false;

            // Loop through the provided roles and add them if they aren't already assigned to the user
            foreach (var role in roleNames)
            {
                // If the user already has the role, we skip it
                if (!user.Roles.Any(r => r.RoleName == role.RoleName))
                {
                    // Check if the role exists in the database
                    var roleFromDb = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == role.RoleName);
                    if (roleFromDb != null)
                    {
                        user.Roles.Add(roleFromDb);
                    }
                }
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<List<Role>> GetRolesForUserAsync(Guid userId)
        {
            return await _context.Users
                         .Where(u => u.Id == userId)
                         .SelectMany(u => u.Roles)
                         .ToListAsync();
        }

        public async Task<bool> RemoveRoleFromUserAsync(Guid userId, string roleName)
        {
            var user = await _context.Users.FindAsync(userId);
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);

            if (user == null || role == null)
                return false;

            user.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
