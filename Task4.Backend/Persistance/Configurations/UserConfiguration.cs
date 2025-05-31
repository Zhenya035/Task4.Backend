using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task4.Backend.Enums;
using Task4.Backend.Models;

namespace Task4.Backend.Persistance.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).ValueGeneratedOnAdd();
        
        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.Status)
            .HasConversion<string>(
                s => s.ToString(),
                str => Enum.Parse<StatusEnum>(str));
    }
}