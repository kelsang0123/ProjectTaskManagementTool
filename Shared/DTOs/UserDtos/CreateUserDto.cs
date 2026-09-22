using System;

namespace DTOs.UserDtos;

public class CreateUserDto
{
 public required String Username{get; set;}
 public required String Password{get; set;}
 public required String FullName{get; set;}
}
