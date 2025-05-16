using Application.Auth.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Auth.Mapping;

public class AuthMappingProfile: Profile
{
    public AuthMappingProfile()
    {
        CreateMap<RegisterUserRequest, User>();
        CreateMap<User, RegisterUserResponse>();
        
        
        CreateMap<User, LoginUserResponse>();
    }
}