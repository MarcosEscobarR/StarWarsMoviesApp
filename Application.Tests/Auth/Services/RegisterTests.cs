using System.Linq.Expressions;
using Application.Auth.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Application.Tests.Auth.Services;

public class AuthServiceTests
{
    private readonly Mock<IAuthRepository> _authRepositoryMock;
    private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IPasswordHasher<User>> _passwordHasherMock;
    private readonly Application.Auth.Services.AuthService _authService;

    public AuthServiceTests()
    {
        _authRepositoryMock = new Mock<IAuthRepository>();
        _jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();
        _mapperMock = new Mock<IMapper>();
        _passwordHasherMock = new Mock<IPasswordHasher<User>>();

        _authService = new Application.Auth.Services.AuthService(
            _authRepositoryMock.Object,
            _jwtTokenGeneratorMock.Object,
            _mapperMock.Object,
            _passwordHasherMock.Object
        );
    }

    [Fact]
    public async Task RegisterUser_ReturnsSuccess_WhenUserIsCreated()
    {
        var request = new RegisterUserRequest("test@test.com", "test", "test", "password123", "User");
        var userEntity = new User(
            request.Name,
            request.Lastname,
            request.Email,
            "hashed-password"
        );
        var createdUser = userEntity;
        var response = new RegisterUserResponse(createdUser.Id, createdUser.Email, createdUser.FullName);   

        _mapperMock.Setup(m => m.Map<User>(request)).Returns(userEntity);
        _passwordHasherMock.Setup(p => p.HashPassword(userEntity, request.Password)).Returns("hashed-password");
        _authRepositoryMock.Setup(r => r.CreateUser(userEntity, default)).ReturnsAsync(createdUser);
        _mapperMock.Setup(m => m.Map<RegisterUserResponse>(createdUser)).Returns(response);

        var result = await _authService.RegisterUser(request);
        var data = (RegisterUserResponse)result.Data!;
        
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(response.Email, data.Email);
        Assert.Equal(response.FullName, data.FullName);
    }
    
    [Fact]
    public async Task RegisterUser_ReturnsFailure_WhenExceptionIsThrown()
    {
        var request = new RegisterUserRequest("test@test.com", "test", "test", "password123", "User");

        _mapperMock.Setup(m => m.Map<User>(request)).Throws(new Exception("Mapping failed"));

        var result = await _authService.RegisterUser(request);

        Assert.False(result.IsSuccess);
        Assert.Contains("An error occurred while creating the user", result.Message);
    }

    [Fact]
    public async Task RegisterUser_ReturnsFailure_WhenUserAlreadyExists()
    {
        var request = new RegisterUserRequest("test@test.com", "test", "test", "password123", "User");
        var existingUser = new User(
            request.Name,
            request.Lastname,
            request.Email,
            "hashed-password"
        );

        _authRepositoryMock
            .Setup(r => r.GetUserBy(It.IsAny<Expression<Func<User, bool>>>(), default))
            .ReturnsAsync(existingUser);

        var result = await _authService.RegisterUser(request);
        
        Assert.False(result.IsSuccess);
        Assert.Equal("Email already in use", result.Message);
    }
}
