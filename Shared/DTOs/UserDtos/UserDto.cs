using System;

namespace DTOs.UserDtos;

public class UserDto
{
    public int Id{get; set;}
    public required String FirstName{get; set;}
    public required String LastName{get; set;}
    public required String Email{get; set;}
}
