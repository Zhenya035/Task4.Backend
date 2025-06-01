using Task4.Backend.Models;

namespace Task4.Backend.Interfaces.Services;

public interface IJwtService
{
    public string GenerateJwtToken(User user);
}