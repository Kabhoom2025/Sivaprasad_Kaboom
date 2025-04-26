using Event___Task_Scheduler_for_Remote_Teams.Models;

namespace Event___Task_Scheduler_for_Remote_Teams.Services.Interfaces
{
    public interface ITeamService
    {
        Task<Team> CreateTeamAsync(Team team);
        Task<List<Team>> GetAllTeamsAsync();
        Task<Team?> GetTeamByIdAsync(Guid id);
        Task<bool> DeleteTeamAsync(Guid id);
    }
}
