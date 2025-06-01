using Task4.Backend.Dtos.Requests;
using Task4.Backend.Dtos.Response;

namespace Task4.Backend.Interfaces.Services;

public interface IUserService
{
    public Task Registration(RegistrationUserDto registrationUser);
    public Task Login(LoginUserDto loginUser);
    public Task<List<GetUserDto>> GetUsersWithoutYou(uint id);
    public Task Delete(List<uint> users);
    public Task Block(List<uint> users);
    public Task UnBlock(List<uint> users);
}