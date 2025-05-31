using Microsoft.EntityFrameworkCore;
using Task4.Backend.Models;
using Task4.Backend.Persistance.Configurations;

namespace Task4.Backend.Persistance;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}