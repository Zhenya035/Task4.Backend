using Microsoft.EntityFrameworkCore;
using Task4.Backend.Persistance;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddDbContext<AppDbContext>(
    options =>
    {
        options.UseSqlServer(configuration.GetConnectionString(nameof(AppDbContext)));
    }
);

var app = builder.Build();

app.Run();