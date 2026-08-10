using System;

namespace Entities;

public class User
{
    public User()
    {
        
    }
    public User(String email, String password, String fullName)
    {
        Email = email;
        Password = password;
        FullName = fullName;
    }
public int Id{get; set;}
public String Email{get; set;}
public String Password{get; set;}
public String FullName{get; set;}
public List<Project> Projects{get; set;}
public List<ProjectMember> ProjectMembers{get; set;}
}
