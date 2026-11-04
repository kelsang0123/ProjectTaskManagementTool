using System;

namespace DTOs.UserDtos;

public class CreateUserDto
{
 public required String FirstName{get; set;}
 public required String LastName{get; set;}
 public required String Email{get; set;}
 public required String Password{get; set;}
}
