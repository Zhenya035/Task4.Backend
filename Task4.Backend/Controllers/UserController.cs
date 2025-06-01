using Microsoft.AspNetCore.Mvc;
using Task4.Backend.Dtos.Requests;
using Task4.Backend.Dtos.Response;
using Task4.Backend.Interfaces.Services;

namespace Task4.Backend.Controllers;

[ApiController]
[Route("users")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost("registration")]
    public async Task<IActionResult> Registration([FromBody] RegistrationUserDto registrationUser)
    {
        await userService.Registration(registrationUser);
        
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto loginUser)
    {
        await userService.Login(loginUser);
        
        return Ok();
    }

    [HttpGet("{userId}/all")]
    public async Task<ActionResult<List<GetUserDto>>> GetUsers(uint userId)
    {
        var users = await userService.GetUsersWithoutYou(userId);
        
        return Ok(users);
    }

    [HttpPost("delete")]
    public async Task<IActionResult> Delete([FromBody] List<uint> users)
    {
        await userService.Delete(users);
        
        return Ok();
    }

    [HttpPost("block")]
    public async Task<IActionResult> Block([FromBody] List<uint> users)
    {
        await userService.Block(users);
        
        return Ok();
    }

    [HttpPost("unblock")]
    public async Task<IActionResult> UnBlock([FromBody] List<uint> users)
    {
        await userService.UnBlock(users);
        
        return Ok();
    }
}