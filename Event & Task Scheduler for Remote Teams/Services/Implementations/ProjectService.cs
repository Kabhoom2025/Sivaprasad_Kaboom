using Event___Task_Scheduler_for_Remote_Teams.Data;
using Event___Task_Scheduler_for_Remote_Teams.Models;
using Event___Task_Scheduler_for_Remote_Teams.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Event___Task_Scheduler_for_Remote_Teams.Services.Implementations
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;
        public ProjectService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Project> CreateProjectAsync(Project project)
        {
            project.Id = Guid.NewGuid();
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return project;
        }

        public Task<bool> DeleteProjectAsync(Guid id)
        {
            var project = _context.Projects.Find(id);
            if (project == null)
            {
                return Task.FromResult(false);
            }
            _context.Projects.Remove(project);
            _context.SaveChangesAsync();
            return Task.FromResult(true);

        }

        public async Task<List<Project>> GetAllProjectsAsync()
        {
            return await _context.Projects.ToListAsync();
        }

        public async Task<Project?> GetProjectByIdAsync(Guid id)
        {
            return await _context.Projects.FindAsync(id);
        }
    }
}
