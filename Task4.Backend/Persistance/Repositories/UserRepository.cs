using Microsoft.EntityFrameworkCore;
using Task4.Backend.Enums;
using Task4.Backend.Interfaces.Repositories;
using Task4.Backend.Models;

namespace Task4.Backend.Persistance.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User> Registration(User newUser)
    {
        var user = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == newUser.Email);
        
        if(user != null)
            throw new InvalidOperationException("User already exists");
        
        var addedUser = await context.Users.AddAsync(newUser);
        await context.SaveChangesAsync();
        
        return addedUser.Entity;
    }

    public async Task<User> Login(string email, string password)
    {
        var user = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
        
        if(user == null)
            throw new KeyNotFoundException("User not found");
        
        if(!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            throw new Exception("Wrong password");

        return user;
    }

    public async Task<List<User>> GetUsersWithoutYou(uint id)
    {
        return await context.Users
            .AsNoTracking()
            .Where(u => u.Id != id)
            .OrderByDescending(u => u.LastLogin)
            .ToListAsync();
    }

    public async Task Delete(List<uint> users)
    {
        var usersToDelete = await context.Users
            .Where(u => users.Contains(u.Id))
            .ToListAsync();

        context.Users.RemoveRange(usersToDelete);
        await context.SaveChangesAsync();
    }

    public async Task Block(List<uint> users)
    {
        var usersForBlock = await context.Users
            .AsNoTracking()
            .Where(u => users.Contains(u.Id))
            .ToListAsync();
        
        if(usersForBlock == null || usersForBlock.Count == 0)
            throw new KeyNotFoundException("Users not found");

        await context.Users
            .Where(u => users.Contains(u.Id))
            .ExecuteUpdateAsync(u => u
                .SetProperty(u => u.Status, StatusEnum.Blocked));
    }

    public async Task UnBlock(List<uint> users)
    {
        var usersForUnBlock = await context.Users
            .AsNoTracking()
            .Where(u => users.Contains(u.Id))
            .ToListAsync();
        
        if(usersForUnBlock == null || usersForUnBlock.Count == 0)
            throw new KeyNotFoundException("Users not found");

        await context.Users
            .Where(u => users.Contains(u.Id))
            .ExecuteUpdateAsync(u => u
                .SetProperty(u => u.Status, StatusEnum.Active));
    }

    public async Task UpdateLastLogin(uint userId)
    {
        var user = context.Users
            .AsNoTracking()
            .FirstOrDefault(u => u.Id == userId);
        
        if(user == null)
            throw new KeyNotFoundException("User not found");
        var time = DateTime.UtcNow;
        await context.Users
            .Where(u => u.Id == userId)
            .ExecuteUpdateAsync(u => u
                .SetProperty(u => u.LastLogin, time));
    }
}