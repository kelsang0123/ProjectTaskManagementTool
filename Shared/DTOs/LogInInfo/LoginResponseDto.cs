using System;
using DTOs.UserDtos;

namespace DTOs.LogInInfo;

public class LoginResponseDto
{
public string Token{get; set;} = "";
public UserDto User {get; set;} = null!;
}
