using System;

namespace Entities;

public class ProjectMember
{
    public ProjectMember(){}
    public ProjectMember(int projectId, int userId, String role)
    {
        ProjectId = projectId;
        UserId = userId;
        Role = role;
    }
public int Id{get; set;}
public int ProjectId{get; set;}
public int UserId{get; set;}
public String Role{get; set;}
public Project Project{get; set;}
public User User{get; set;}

}
