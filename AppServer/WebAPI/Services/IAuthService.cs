using System;
using Entities;

namespace WebAPI.Services;

public interface IAuthService
{

    Task<User> ValidateUser(string email, string password);
    Task RegisterUser(User user);
}
