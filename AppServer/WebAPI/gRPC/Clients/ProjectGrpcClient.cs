using DTOs;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using WebAPI.Protos;

namespace WebAPI.gRPC;

public class ProjectGrpcClient
{
    private readonly ProjectService.ProjectServiceClient client;

    public ProjectGrpcClient()
    {
        var channel = 
        GrpcChannel.ForAddress(
            "http://localhost:7891/"
        );

        client = new ProjectService.ProjectServiceClient(channel);
    }

      public async Task<ProjectDto> RegisterProject(
        CreateProjectDto dto)
    {

        var request =
            new RegisterProjectRequest
            {
                Title = dto.Title,
                Description = dto.Description,
                CreatorId = dto.CreatorId,
            };


        RegisterProjectResponse response =
            await client.RegisterProjectAsync(
                request
            );


        return MapToProjectDTO(response.Project);
    }

      public async Task<ProjectDto?> GetProject(
        int id)
    {

        var request = new GetProjectRequest
        {
            Id = id
        };


        GetProjectResponse response =
            await client.getProjectAsync(request);


        if(response.Project == null)
            return null;


        return MapToProjectDTO(response.Project);
    }

     public async Task<IEnumerable<ProjectDto>> GetProjects()
    {
       try
       {
        var request = new GetProjectsRequest();


        GetProjectsResponse response =
            await client.getProjectsAsync(request);


        return response.Projects
            .Select(MapToProjectDTO)
            .ToList();
    }
    catch(Grpc.Core.RpcException ex)
        {
            Console.WriteLine($"gRPC error: {ex.StatusCode}");
            Console.WriteLine($"gRPC detail: {ex.Status.Detail}");
            Console.WriteLine($"gRPC debug: {ex}");

            throw;
        }
    }

    private ProjectDto MapToProjectDTO(DTOProject project)
    {
          return new ProjectDto
        {
            Id = project.Id,
            Title = project.Title,
            Description = project.Description,
            Status = project.Status,
            CreatorId = project.CreatorId,

            CreatedAt =
                project.CreatedAt.ToDateTime().ToLocalTime()
        };
    }
}
