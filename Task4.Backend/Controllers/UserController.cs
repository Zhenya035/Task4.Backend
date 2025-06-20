﻿using Microsoft.AspNetCore.Authorization;
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
    public async Task<ActionResult<AuthorizationDto>> Registration([FromBody] RegistrationUserDto registrationUser)
    {
        var response = await userService.Registration(registrationUser);
        
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthorizationDto>> Login([FromBody] LoginUserDto loginUser)
    {
        var response = await userService.Login(loginUser);
        
        return Ok(response);
    }

    [HttpGet("{userId}/all")]
    [Authorize(Policy = "ActiveOnly")]
    public async Task<ActionResult<List<GetUserDto>>> GetUsers(uint userId)
    {
        var users = await userService.GetUsersWithoutYou(userId);
        
        return Ok(users);
    }

    [HttpPost("delete")]
    [Authorize(Policy = "ActiveOnly")]
    public async Task<IActionResult> Delete([FromBody] List<uint> users)
    {
        await userService.Delete(users);
        
        return Ok();
    }

    [HttpPost("block")]
    [Authorize(Policy = "ActiveOnly")]
    public async Task<IActionResult> Block([FromBody] List<uint> users)
    {
        await userService.Block(users);
        
        return Ok();
    }

    [HttpPost("unblock")]
    [Authorize(Policy = "ActiveOnly")]
    public async Task<IActionResult> UnBlock([FromBody] List<uint> users)
    {
        await userService.UnBlock(users);
        
        return Ok();
    }
}