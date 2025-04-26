using Event___Task_Scheduler_for_Remote_Teams.Models;
using Event___Task_Scheduler_for_Remote_Teams.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Event___Task_Scheduler_for_Remote_Teams.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        
        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
          
        }
        [HttpGet("GetAllProjects")]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            return Ok(projects);
        }
        [HttpGet("GetProject/{id}")]
        public async Task<IActionResult> GetProjectById(Guid id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            var result = await _projectService.GetProjectByIdAsync(id);
            return result != null ? Ok(result) : NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("CreateProject")]
        public async Task<IActionResult> CreateProject([FromBody] Project project)
        {
            if (project == null)
            {
                return BadRequest();
            }
            //var createdProject = await _projectService.CreateProjectAsync(project);
            //return CreatedAtAction(nameof(GetProjectById), new { id = createdProject.Id }, createdProject);
            var result = await _projectService.CreateProjectAsync(project);
            return Ok(result);
        }
        [HttpDelete("DeleteProject/{id}")]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            var result = await _projectService.DeleteProjectAsync(id);
            return result ? Ok("Deleted") : NotFound();
        }
    }
}
