using Application.Auth.DTOs;
using Shared.Result;

namespace Application.Auth.Services;

public interface IAuthService
{
    Task<ResultBase<RegisterUserResponse>> RegisterUser(RegisterUserRequest request);
    Task<ResultBase<LoginUserResponse>> LoginUser(LoginUserRequest request);
}