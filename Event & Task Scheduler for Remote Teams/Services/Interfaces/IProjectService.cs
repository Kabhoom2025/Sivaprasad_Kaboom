using Event___Task_Scheduler_for_Remote_Teams.Models;

namespace Event___Task_Scheduler_for_Remote_Teams.Services.Interfaces
{
    public interface IProjectService
    {
        Task<Project> CreateProjectAsync(Project project);
        Task<List<Project>> GetAllProjectsAsync();
        Task<Project?> GetProjectByIdAsync(Guid id);
        Task<bool> DeleteProjectAsync(Guid id);
    }
}
