using Task4.Backend.Dtos.Requests;
using Task4.Backend.Dtos.Response;
using Task4.Backend.Interfaces.Repositories;
using Task4.Backend.Interfaces.Services;
using Task4.Backend.Mappings;

namespace Task4.Backend.Services;

public class UserService(IUserRepository repository, IJwtService jwtService) : IUserService
{
    public async Task<AuthorizationDto> Registration(RegistrationUserDto registrationUser)
    {
        var user = await repository.Registration(UserMapping.MapFromRegistrationDto(registrationUser));
        var token = jwtService.GenerateJwtToken(user);
        await UpdateLastLogin(user.Id);

        return new AuthorizationDto
        {
            UserId = user.Id,
            Token = token
        };
    }

    public async Task<AuthorizationDto> Login(LoginUserDto loginUser)
    {
        var user = await repository.Login(loginUser.Email, loginUser.Password);
        var token = jwtService.GenerateJwtToken(user);

        await UpdateLastLogin(user.Id);
        
        return new AuthorizationDto
        {
            UserId = user.Id,
            Token = token
        };
    }

    public async Task<List<GetUserDto>> GetUsersWithoutYou(uint id)
    {
        var users = await repository.GetUsersWithoutYou(id);

        return users.Select(UserMapping.MapToGetUserDto).ToList();
    }

    public async Task Delete(List<uint> users)
    {
        if(users.Count == 0)
            throw new ArgumentException("Users cannot be empty");
        
        await repository.Delete(users);
    }

    public async Task Block(List<uint> users)
    {
        if(users.Count == 0)
            throw new ArgumentException("Users cannot be empty");
        
        await repository.Block(users);
    }

    public async Task UnBlock(List<uint> users)
    {
        if(users.Count == 0)
            throw new ArgumentException("Users cannot be empty");
        
        await repository.UnBlock(users);
    }

    public async Task UpdateLastLogin(uint userId)
    {
        await repository.UpdateLastLogin(userId);
    }
}