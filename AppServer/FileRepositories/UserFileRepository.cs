using System;
using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class UserFileRepository : IUserRepository
{
    private readonly string filePath = "users.json";

    public UserFileRepository()
    {
        if(!File.Exists(filePath))
        {
            File.WriteAllText(filePath,"[]");
        }
    }
    public async Task<User> AddAsync(User user)
    {
        List<User> users = await LoadUsers();
        int maxId = users.Count > 0 ? users.Max(u=>u.Id) : 0;
        user.Id = maxId + 1;
        users.Add(user);
        await SaveList(users);
        return user;
    }

    public async Task DeleteAsync(int id)
    {
        List<User> users = await LoadUsers();
        User? userToRemove = users.SingleOrDefault(u=>u.Id == id);
        if(userToRemove is null)
        {
            throw new KeyNotFoundException($"User with ID {id} not found");
        }
        users.Remove(userToRemove);
        await SaveList(users);
    }

    public IQueryable<User> GetMany()
    {
        return LoadUsers().Result.AsQueryable();
    }

    public async Task<User> GetSingleAsync(int id)
    {
        List<User> users = await LoadUsers();
        User user = users.SingleOrDefault(u=>u.Id == id)!;
        if(user is null)
        {
            throw new KeyNotFoundException($"User with ID {id} not found");
        }
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        List<User> users = await LoadUsers();
        User? existingUser = users.Single(u=>u.Id == user.Id);
        if(existingUser is null)
        {
            throw new KeyNotFoundException($"User with ID {user.Id} not found");
        }
        users.Remove(existingUser);
        users.Add(user);
        await SaveList(users);
    }
     private async Task<List<User>> LoadUsers()
    {
        string json = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(json)!;
        return users;
    }
    private async Task SaveList(List<User> users)
    {
        string json = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(filePath, json);
    }
}
