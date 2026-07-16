using DTOs;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using WebAPI.gRPC;
using WebAPI.gRPC.Interfaces;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
      //  private readonly IProjectRepository projectRepo;
      private readonly IProjectService projectService;

        public ProjectsController(IProjectService projectService)
        {
            this.projectService = projectService;
        }


        [HttpPost("createProject")]
        public async Task<ActionResult<ProjectDto>> AddProject([FromBody] CreateProjectDto request)
        {
            try
            {
                ProjectDto created = await projectService.CreateProject(request);

                return Created(
                    $"/projects/{created.Id}", created
                );
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProjectDto>> GetProject(
        [FromRoute] int id)
       {
        ProjectDto? project =
            await projectService.GetById(id);

        if(project == null)
        {
            return NotFound();
        }
        return Ok(project);
    }

        [HttpGet("manyProjects")]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects()
        {
            IEnumerable<ProjectDto> projects = await projectService.GetMany();
            return Ok(projects);
        }

    }
}
