using Application.Auth.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterUserRequest, User>();
        CreateMap<User, LoginUserResponse>();
    }
}