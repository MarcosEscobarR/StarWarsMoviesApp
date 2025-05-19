using System.ComponentModel.DataAnnotations;

namespace Application.Auth.DTOs;

public record RegisterUserRequest(

    string Email,

    string Name,

    string Lastname,
    
    string Password,
    
    string UserRole
    );
public record RegisterUserResponse(
    Guid Id,
    
    string Email,
    
    string FullName
    );
public record LoginUserRequest(string Email, string Password);
public record LoginUserResponse(string Token);