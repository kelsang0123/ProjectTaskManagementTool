using System.Text.Json;
using DTOs;

namespace BlazorApp.Services;

public class HttpProjectService : IProjectService
{
private readonly HttpClient client;

public HttpProjectService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<ProjectDto> AddProjectAsync(CreateProjectDto request)
    {
        HttpResponseMessage httpResponse = await client.PostAsJsonAsync("projects", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if(!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<ProjectDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task DeleteProjectAsync(int id)
    {
        HttpResponseMessage httpResponse = await client.DeleteAsync("$projects/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if(!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
    }

    public async Task<ICollection<ProjectDto>> GetProjectsAsync()
    {
        HttpResponseMessage response = await client.GetAsync("projects");
        string content = await response.Content.ReadAsStringAsync();

        if(!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}, {content}");
        }
        JsonSerializerOptions options = new()
        {
            PropertyNameCaseInsensitive = true
        };
        var projects = JsonSerializer.Deserialize<ICollection<ProjectDto>>(content, options) ?? [];
        return projects;
    }

}
