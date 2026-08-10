using System;
using System.ComponentModel.DataAnnotations;
using Entities;
using RepositoryContracts;
using WebAPI.Controllers;

namespace WebAPI.Services;

public class AuthService : IAuthService
{
private readonly IUserRepository userRepo;

public AuthService(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    public async Task RegisterUser(User user)
    {
        if(string.IsNullOrEmpty(user.Email))
        {
            throw new ValidationException("Email cannot be null");
        }
        if(string.IsNullOrEmpty(user.Password))
        {
            throw new ValidationException("Password cannot be null");
        }
        await userRepo.AddAsync(user);
    }

    public async Task<User> ValidateUser(string email, string password)
    {
         User? existingUser = userRepo.GetMany().FirstOrDefault(u=>
          u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)) ?? throw new Exception("User not found");

        if(existingUser == null)
        {
            throw new Exception("User not found");
        }
        if(!(existingUser.Email.Contains("@") || existingUser.Email.Contains(".com")))
        {
            throw new Exception("Email doesnot have correct format!");
        }
          if(!existingUser.Password.Equals(password))
        {
            throw new Exception("Password mismatch");
        }
        return await Task.FromResult(existingUser);
    }
}
