namespace Application.Auth.DTOs;

public record RegisterUserRequest(string Email, string Name, string Lastname, string Password);
public record LoginUserRequest(string Email, string Password);
public record LoginUserResponse(string Token);