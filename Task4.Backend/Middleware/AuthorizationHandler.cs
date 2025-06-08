using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Task4.Backend.Enums;
using Task4.Backend.Persistance;

namespace Task4.Backend.Middleware;

public class ActiveUserRequirement : IAuthorizationRequirement;

public class ActiveUserHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    : AuthorizationHandler<ActiveUserRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context1,
        ActiveUserRequirement requirement)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null) return;

        var userIdClaim = httpContext.User.FindFirst("id");
        if (userIdClaim == null) return;

        if (!int.TryParse(userIdClaim.Value, out var userId)) return;

        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user != null && user.Status == StatusEnum.Active)
        {
            context1.Succeed(requirement);
        }
    }
}