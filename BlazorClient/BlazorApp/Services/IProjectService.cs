using DTOs;

namespace BlazorApp.Services;

public interface IProjectService
{
public Task<ProjectDto> AddProjectAsync(CreateProjectDto request);
public Task<ICollection<ProjectDto>> GetProjectsAsync();
public Task DeleteProjectAsync(int id);
}
