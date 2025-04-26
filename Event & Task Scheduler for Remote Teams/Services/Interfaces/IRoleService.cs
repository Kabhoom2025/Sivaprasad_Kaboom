using Event___Task_Scheduler_for_Remote_Teams.Models;

namespace Event___Task_Scheduler_for_Remote_Teams.Services.Interfaces
{
    public interface IRoleService
    {
        Task<List<Role>> GetRolesForUserAsync(Guid userId);
        Task<bool> AddRoleToUserAsync(Guid userId, string roleName);
        Task<bool> RemoveRoleFromUserAsync(Guid userId, string roleName);
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<bool> AssignMultipleRolesToUserAsync(Guid userId, List<Role> roleNames);
    }
}
