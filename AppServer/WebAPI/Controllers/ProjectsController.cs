using DTOs;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectRepository projectRepo;

        public ProjectsController(IProjectRepository projectRepo)
        {
            this.projectRepo = projectRepo;
        }

        [HttpPost("createProject")]
        public async Task<ActionResult<ProjectDto>> AddProject([FromBody] CreateProjectDto request)
        {
            await VerifyProjectTitleIsAvailable(request.Title);
            DateTime dateTime = DateTime.Now;
            Project project = new(request.Title, request.Description, "Not Started", dateTime);
            Project created = await projectRepo.AddAsync(project);
            ProjectDto dto = new()
            {
                Id = created.Id,
                Title = created.Title,
                Description = created.Description,
                Status = "Not Started",
                CreatedAt = dateTime
            };
            return Created($"/projects/{dto.Id}", created);
        }

        private async Task VerifyProjectTitleIsAvailable(string title)
        {
            List<Project> projects = projectRepo.GetMany().AsQueryable().ToList();
            Project? existingProject = projects.SingleOrDefault(p=>p.Title.Equals(title));
            if(existingProject!=null)
            {
                throw new KeyNotFoundException("Title already exists!");
            }
        }

        [HttpGet("manyProjects")]
        public async Task<IResult> GetProjects(
            [FromQuery] string? status = null,
            [FromQuery] int? projectId = null
        )
        {
            IQueryable<Project> queryableProjects = projectRepo.GetMany();
            if(status!=null)
            {
                queryableProjects = queryableProjects.Where(p=>p.Status.Contains(status));
            }
            if(projectId != null)
            {
                queryableProjects = queryableProjects.Where(p=>p.Id == projectId);
            }
            List<ProjectDto> projects = queryableProjects.Select(project => new ProjectDto
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                Status = project.Status,
                CreatedAt = project.CreatedAt
            })
            .ToList();
            return Results.Ok(projects);
        }

        [HttpDelete("{projectId:int}/deleteProject")]
        public async Task<ActionResult> DeleteProject([FromRoute] int projectId)
        {
            await projectRepo.DeleteAsync(projectId);
            return NoContent();
        }
    }
}
