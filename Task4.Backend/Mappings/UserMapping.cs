using Task4.Backend.Dtos.Requests;
using Task4.Backend.Dtos.Response;
using Task4.Backend.Models;

namespace Task4.Backend.Mappings;

public class UserMapping
{
    public static User MapFromRegistrationDto(RegistrationUserDto registrationUser) =>
        new User()
        {
            Name = registrationUser.Name,
            Email = registrationUser.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registrationUser.Password)
        };

    public static GetUserDto MapToGetUserDto(User user) =>
        new GetUserDto()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            LastLogin = user.LastLogin,
            Status = user.Status.ToString(),
            CreatedAt = user.CreatedAt,
        };
}