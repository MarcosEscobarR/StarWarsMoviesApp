using Application.Auth.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Shared.Result;

namespace Application.Auth.Services;

public class AuthService(IAuthRepository authRepository, IJwtTokenGenerator jwtTokenGenerator, IMapper mapper, IPasswordHasher<User> passwordHasher)
{
    public async Task<ResultBase<RegisterUserResponse>> RegisterUser(RegisterUserRequest request)
    {
        try
        {
            var user = mapper.Map<User>(request);
            user.Password = passwordHasher.HashPassword(user, request.Password);
            
            var result = await authRepository.CreateUser(user);
            if (result is null)
            {
                return ResultBuilder.IsFailure<RegisterUserResponse>("User creation failed");
            }
            var response = mapper.Map<RegisterUserResponse>(result);
            return ResultBuilder.IsOk(response);
        }
        
        catch (Exception e)
        {
            return ResultBuilder.IsFailure<RegisterUserResponse>("An error occurred while creating the user: " + e.Message);
        }
    }

    public async Task<ResultBase<LoginUserResponse>> LoginUser(LoginUserRequest request)
    {
        try
        {
            var user = await authRepository.GetUserBy(u => u.Email == request.Email);
            if (user is null)
            {
                return ResultBuilder.IsFailure<LoginUserResponse>("User not found");
            }
            
            if (!VerifyPassword(user, request.Password))
            {
                return ResultBuilder.IsFailure<LoginUserResponse>("Invalid password");
            }
            
            var token = jwtTokenGenerator.GenerateToken(user);
            return ResultBuilder.IsOk(new LoginUserResponse(token));
        }
        catch (Exception e)
        {
            return ResultBuilder.IsFailure<LoginUserResponse>("An error occurred while logging in: " + e.Message);
        }
    }
    
    private bool VerifyPassword(User user,string password)
    {
        var result = passwordHasher.VerifyHashedPassword(user, user.Password, password);   
        return result == PasswordVerificationResult.Success;
    }
}