using Task4.Backend.Models;

namespace Task4.Backend.Interfaces.Repositories;

public interface IUserRepository
{
    public Task Registration(User newUser);
    public Task Login(string email, string password);
    public Task<List<User>> GetUsersWithoutYou(uint id);
    public Task Delete(List<uint> users);
    public Task Block(List<uint> users);
    public Task UnBlock(List<uint> users);
}