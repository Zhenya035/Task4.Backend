using Task4.Backend.Enums;

namespace Task4.Backend.Models;

public class User
{
    public uint Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime? LastLogin { get; set; }
    public StatusEnum Status { get; set; } = StatusEnum.Active;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}