using System.Linq.Expressions;
using Application.Auth.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Application.Tests.Auth.Services;

public class LoginTests
{
    private readonly Mock<IAuthRepository> _authRepositoryMock;
    private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IPasswordHasher<User>> _passwordHasherMock;
    private readonly Application.Auth.Services.AuthService _authService;

    public LoginTests()
    {
        _authRepositoryMock = new Mock<IAuthRepository>();
        _jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();
        _mapperMock = new Mock<IMapper>();
        _passwordHasherMock = new Mock<IPasswordHasher<User>>();

        _authService = new Application.Auth.Services.AuthService(
            _authRepositoryMock.Object,
            _jwtTokenGeneratorMock.Object,
            _mapperMock.Object,
            _passwordHasherMock.Object);
    }

    [Fact]
    public async Task LoginUser_LoginSuccess()
    {
        var mockToken = "token";
        var request = new LoginUserRequest("test@test.com", "hashed-password");
        var entity = new User("test", "test", request.Email,request.Password);
        var response = new LoginUserResponse(mockToken);

        _passwordHasherMock.Setup(p => p.VerifyHashedPassword(entity, entity.Password, request.Password))
            .Returns(PasswordVerificationResult.Success);
        _authRepositoryMock.Setup(r => r.GetUserBy(It.IsAny<Expression<Func<User, bool>>>(), default))
            .ReturnsAsync(entity);
        _jwtTokenGeneratorMock.Setup(j => j.GenerateToken(entity))
            .Returns(mockToken);
        _mapperMock.Setup(m => m.Map<LoginUserResponse>(It.IsAny<User>()))
            .Returns(response);
        
        var result = await _authService.LoginUser(request);
        var data = (LoginUserResponse)result.Data!;
        
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(mockToken, data.Token);
    }

    [Fact]
    public async Task LoginUser_UserNotFound()
    {
        var request = new LoginUserRequest("test@test.com", "hashed-password");
         _authRepositoryMock.Setup(r => r.GetUserBy(It.IsAny<Expression<Func<User, bool>>>(), default))
            .ReturnsAsync((User?)null);

        var result = await _authService.LoginUser(request);
        var data = (LoginUserResponse)result.Data!;
        
        Assert.False(result.IsSuccess);
        Assert.Null(data);
        Assert.Equal("User not found", result.Message);
    }

    [Fact]
    public async Task LoginUser_InvalidPassword()
    {
        var request = new LoginUserRequest("test@test.com", "hashed-password");
        var entity = new User("test", "test", request.Email, request.Password);

        _authRepositoryMock.Setup(r => r.GetUserBy(It.IsAny<Expression<Func<User, bool>>>(), default))
            .ReturnsAsync(entity);
        _passwordHasherMock.Setup(p => p.VerifyHashedPassword(entity, entity.Password, request.Password))
            .Returns(PasswordVerificationResult.Failed);

        var result = await _authService.LoginUser(request);
        var data = (LoginUserResponse)result.Data!;

        Assert.False(result.IsSuccess);
        Assert.Null(data);
        Assert.Equal("Invalid password", result.Message);
    }
}