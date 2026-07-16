using System;
using DTOs;

namespace WebAPI.gRPC.Interfaces;

public interface IProjectService
{
        Task<ProjectDto> CreateProject(CreateProjectDto dto);      
        Task<ProjectDto?> GetById(int id);
        Task<IEnumerable<ProjectDto>> GetMany();
}
