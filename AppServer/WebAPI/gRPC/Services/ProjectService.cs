using System;
using DTOs;
using WebAPI.gRPC.Interfaces;

namespace WebAPI.gRPC.Services;

public class ProjectService : IProjectService
{
      private readonly ProjectGrpcClient grpcClient;


    public ProjectService(
        ProjectGrpcClient grpcClient)
    {
        this.grpcClient = grpcClient;
    }

    public async Task<ProjectDto> CreateProject(CreateProjectDto dto)
    {
        return await grpcClient.RegisterProject(
        dto
        );
    }

    
    public async Task<ProjectDto?> GetById(int id)
    {
        return await grpcClient.GetProject(id);
    }

    public Task<IEnumerable<ProjectDto>> GetMany() //No IQueryable since the appserver doesnot directly interact with database.
    {
        return grpcClient.GetProjects();
    }
}
