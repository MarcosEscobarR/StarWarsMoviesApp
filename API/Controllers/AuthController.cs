using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController: ControllerBase
{
    [HttpPost("register")]
    public Task<IActionResult> Register()
    {
        return Task.FromResult<IActionResult>(Ok("User registered successfully"));
    }
    
    [HttpPost("login")]
    public Task<IActionResult> Login()
    {
        return Task.FromResult<IActionResult>(Ok("User logged in successfully"));
    }
    
    [HttpPost("logout")]
    public Task<IActionResult> Logout()
    {
        return Task.FromResult<IActionResult>(Ok("User logged out successfully"));
    }
}