using Application.Auth.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Auth.Mapping;

public class AuthMappingProfile: Profile
{
    public AuthMappingProfile()
    {
        CreateMap<RegisterUserRequest, User>()
            .ForMember(dest => dest.Role,
                opt => opt.MapFrom(src => src.UserRole == "Admin" ? UserRole.Admin : UserRole.User));
        CreateMap<User, RegisterUserResponse>();
        
        CreateMap<User, LoginUserResponse>();
    }
}