using API.Extensions;
using Application.Auth.DTOs;
using Application.Auth.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(AuthService authService) : ControllerBase
{
    private readonly AuthService _authService = authService;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserRequest request)
    {
        var result = await _authService.RegisterUser(request);
        return result.ToActionResult();
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserRequest request)
    {
        var result = await _authService.LoginUser(request);
        return result.ToActionResult();
    }
    
    [HttpPost("logout")]
    public Task<IActionResult> Logout()
    {
        return Task.FromResult<IActionResult>(Ok("User logged out successfully"));
    }
}