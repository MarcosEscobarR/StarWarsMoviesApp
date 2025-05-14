using Application.Auth.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Shared.Result;

namespace Application.Auth.Services;

public class AuthService(IAuthRepository authRepository, IMapper mapper)
{
    public async Task<ResultBase<User>> RegisterUser(RegisterUserRequest request)
    {
        try
        {
            var user = mapper.Map<User>(request);
            var result = await authRepository.CreateUser(user);
            return result is null 
                ? ResultBuilder.IsFailure<User>("User creation failed") 
                : ResultBuilder.IsOk(result);
        }
        
        catch (Exception e)
        {
            return ResultBuilder.IsFailure<User>("An error occurred while creating the user: " + e.Message);
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
            
            // Here you would typically check the password and generate a token
            var token = "GeneratedToken";
            return ResultBuilder.IsOk(new LoginUserResponse(token));
        }
        catch (Exception e)
        {
            return ResultBuilder.IsFailure<LoginUserResponse>("An error occurred while logging in: " + e.Message);
        }
    }
}