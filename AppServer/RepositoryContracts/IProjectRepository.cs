using Entities;

namespace RepositoryContracts;

public interface IProjectRepository
{
  Task<Project> AddAsync(Project project);
  Task UpdateAsync(Project project);
  Task DeleteAsync(int id);
  Task<Project> GetSingleAsync(int id);
  IQueryable<Project> GetMany();
}
