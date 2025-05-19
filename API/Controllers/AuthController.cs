using API.Extensions;
using Application.Auth.DTOs;
using Application.Auth.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Result;

namespace API.Controllers;

/// <summary>
/// Controlador para operaciones de autenticación (registro e inicio de sesión).
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    /// <summary>
    /// Registra un nuevo usuario en el sistema.
    /// </summary>
    /// <param name="request">Los datos para el nuevo usuario.</param>
    /// <returns>Un resultado que indica el éxito o fracaso de la operación.</returns>
    /// <response code="200">Usuario registrado correctamente.</response>
    /// <response code="400">Error de validación o de negocio.</response>
    /// <response code="500">Error interno del servidor.</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(RegisterUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultBase), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResultBase), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        var result = await authService.RegisterUser(request);
        return result.ToActionResult();
    }

    /// <summary>
    /// Inicia sesión en el sistema con credenciales de usuario.
    /// </summary>
    /// <param name="request">Las credenciales del usuario.</param>
    /// <returns>Un token JWT si la autenticación es exitosa.</returns>
    /// <response code="200">Inicio de sesión exitoso.</response>
    /// <response code="400">Credenciales inválidas.</response>
    /// <response code="500">Error interno del servidor.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultBase), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResultBase), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
    {
        var result = await authService.LoginUser(request);
        return result.ToActionResult();
    }
}
