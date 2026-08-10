using System;
using Entities;

namespace RepositoryContracts;

public interface IProjectMemberRepository
{
    Task<ProjectMember> AddAsync(ProjectMember member);
    Task UpdateAsync(ProjectMember member);
    Task DeleteAsync(int id);
    Task<ProjectMember> GetSingleAsync(int id);
    IQueryable<ProjectMember> GetMany();

}
