using Microsoft.EntityFrameworkCore;
using Task4.Backend.Enums;
using Task4.Backend.Interfaces.Repositories;
using Task4.Backend.Models;

namespace Task4.Backend.Persistance.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task Registration(User newUser)
    {
        var user = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == newUser.Email);
        
        if(user != null)
            throw new InvalidOperationException("User already exists");
        
        await context.Users.AddAsync(newUser);
        await context.SaveChangesAsync();
    }

    public async Task Login(string email, string password)
    {
        var user = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
        
        if(user == null)
            throw new KeyNotFoundException("User not found");
        
        if(!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            throw new Exception("Wrong password");
    }

    public async Task<List<User>> GetUsersWithoutYou(uint id)
    {
        return await context.Users
            .AsNoTracking()
            .Where(u => u.Id != id)
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
}