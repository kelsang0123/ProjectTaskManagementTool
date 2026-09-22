using System;
using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class ProjectMemberFileRepository : IProjectMemberRepository
{
    private readonly string filePath = "members.json";

    public ProjectMemberFileRepository()
    {
        if(!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    public async Task<ProjectMember> AddAsync(ProjectMember member)
    {
        List<ProjectMember> members = await LoadMembers();
        int maxId = members.Count > 0 ? members.Max(m => m.Id) : 0;
        members.Add(member);
        await SaveList(members);
        return member; 
    }

    public async Task DeleteAsync(int id)
    {
        List<ProjectMember> members = await LoadMembers();
        ProjectMember? memberToRemove = members.SingleOrDefault(m=>m.Id == id);
        if(memberToRemove is null)
        {
            throw new KeyNotFoundException($"Project member with ID {id} not found");
        }
        members.Remove(memberToRemove);
        await SaveList(members);
    }

    public IQueryable<ProjectMember> GetMany()
    {
        return LoadMembers().Result.AsQueryable();
    }

    public async Task<ProjectMember> GetSingleAsync(int id)
    {
      List<ProjectMember> members = await LoadMembers();
      ProjectMember member = members.SingleOrDefault(m=>m.Id == id)!;
      if(member is null)
        {
            throw new KeyNotFoundException($"Project member with ID {id} not found");
        }
        return member;
    }

    public async Task UpdateAsync(ProjectMember member)
    {
        List<ProjectMember> members = await LoadMembers();
        ProjectMember? existingProject = members.SingleOrDefault(m=>m.Id == member.Id);
        if(existingProject is null)
        {
            throw new KeyNotFoundException($"Project member with ID {member.Id} not found");
        }
        members.Remove(existingProject);
        members.Add(member);
        await SaveList(members);
    }
    
    private async Task<List<ProjectMember>> LoadMembers()
    {
       string json = await File.ReadAllTextAsync(filePath);
       List<ProjectMember> members = JsonSerializer.Deserialize<List<ProjectMember>>(json)!;
       return members;
    }
    private async Task SaveList(List<ProjectMember> members)
    {
        string json = JsonSerializer.Serialize(members);
        await File.WriteAllTextAsync(filePath, json);
    }
}
