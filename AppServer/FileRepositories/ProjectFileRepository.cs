using System.Text.Json;
using System.Text.Json.Nodes;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class ProjectFileRepository : IProjectRepository
{
private readonly string filePath = "projects.json";

public ProjectFileRepository()
    {
        if (!File.Exists(filePath))
	        {
	            File.WriteAllText(filePath, "[]");
	        }
    }

    public async Task<Project> AddAsync(Project project)
    {
        List<Project> projects = await LoadProjects();
        int maxId = projects.Count > 0 ? projects.Max(p => p.Id) : 0;
        project.Id = maxId + 1;
        projects.Add(project);
        await SaveList(projects);
        return project;
    }

    public async Task DeleteAsync(int id)
    {
        List<Project> projects = await LoadProjects();
        Project? projectToRemove = projects.SingleOrDefault(p=>p.Id == id);
        if(projectToRemove is null)
        {
            throw new KeyNotFoundException($"Project with ID {id} not found");
        }
        projects.Remove(projectToRemove);
        await SaveList(projects);
    }

    public IQueryable<Project> GetMany()
    {
        return LoadProjects().Result.AsQueryable();
    }

    public async Task<Project> GetSingleAsync(int id)
    {
        List<Project> projects = await LoadProjects();
        Project project = projects.SingleOrDefault(p=>p.Id == id)!;
        if(project is null)
        {
            throw new KeyNotFoundException($"Project with ID {id} not found");
        }
        return project;
    }

    public async Task UpdateAsync(Project project)
    {
        List<Project> projects = await LoadProjects();
        Project? existingProject = projects.Single(p=>p.Id == project.Id);
        if(existingProject is null)
        {
            throw new KeyNotFoundException($"Project with ID {project.Id} not found");
        }
        projects.Remove(existingProject);
        projects.Add(project);
        await SaveList(projects);
    }

    private async Task<List<Project>> LoadProjects()
    {
        string json = await File.ReadAllTextAsync(filePath);
        List<Project> projects = JsonSerializer.Deserialize<List<Project>>(json)!;
        return projects;
    }
    private async Task SaveList(List<Project> projects)
    {
        string json = JsonSerializer.Serialize(projects);
        await File.WriteAllTextAsync(filePath, json);
    }
}
