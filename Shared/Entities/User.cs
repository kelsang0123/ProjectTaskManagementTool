using System;

namespace Entities;

public class User
{
    public User()
    {
        
    }
    public User(String firstname, String lastname, String email, String password)
    {
        Email = email;
        Password = password;
        FirstName = firstname;
        LastName = lastname;
    }
public int Id{get; set;}
public String Email{get; set;}
public String Password{get; set;}
public String FirstName{get; set;}
public String LastName{get; set;}
}
