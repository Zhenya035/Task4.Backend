using Microsoft.EntityFrameworkCore;
using Task4.Backend.Interfaces.Repositories;
using Task4.Backend.Interfaces.Services;
using Task4.Backend.JwtTokens;
using Task4.Backend.Middleware;
using Task4.Backend.Persistance;
using Task4.Backend.Persistance.Repositories;
using Task4.Backend.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.Configure<AuthSettings>(configuration.GetSection("AuthSettings"));
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddJwtTokens(configuration);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ActiveOnly", policy => 
        policy.RequireClaim("status", "Active"));
});

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

app.UseCors(policy => policy
    .WithOrigins("http://localhost:5173")
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();