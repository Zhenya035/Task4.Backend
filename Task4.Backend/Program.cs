using Microsoft.EntityFrameworkCore;
using Task4.Backend.Interfaces.Repositories;
using Task4.Backend.Interfaces.Services;
using Task4.Backend.Middleware;
using Task4.Backend.Persistance;
using Task4.Backend.Persistance.Repositories;
using Task4.Backend.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddDbContext<AppDbContext>(
    options =>
    {
        options.UseSqlServer(configuration.GetConnectionString(nameof(AppDbContext)));
    }
);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();