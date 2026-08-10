using System;

namespace DTOs.LogInInfo;

public class UserLoginDto
{
    public UserLoginDto(string email, string password)
    {
        this.Email = email;
        this.Password = password;
    }
    public string Email{get; set;} = "";
    public string Password{get; set;} = "";
}
