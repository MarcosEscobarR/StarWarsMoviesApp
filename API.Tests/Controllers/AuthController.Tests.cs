using System.Text.Json;
using API.Controllers;
using Application.Auth.DTOs;
using Application.Auth.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shared.Result;

namespace API.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _authServiceMock = new Mock<IAuthService>();
        _controller = new AuthController(_authServiceMock.Object);
    }

    [Fact]
    public async Task Register_ShouldReturnOk_WhenRegistrationIsSuccessful()
    {
        var request = new RegisterUserRequest("test@test.com", "test", "test", "hashed-password", "User");
        var response = new RegisterUserResponse(Guid.NewGuid(), request.Email, $"{request.Name} {request.Lastname}");

        _authServiceMock.Setup(a => a.RegisterUser(request))
            .ReturnsAsync(ResultBuilder.IsOk(response));

        var result = await _controller.Register(request);

        var okResult = Assert.IsType<OkObjectResult>(result);
    
        // serializamos y deserializamos para extraer el .data
        var json = JsonSerializer.Serialize(okResult.Value);
        using var doc = JsonDocument.Parse(json);
        var data = doc.RootElement.GetProperty("Data");

        Assert.Equal(response.Email, data.GetProperty("Email").GetString());
        Assert.Equal(response.FullName, data.GetProperty("FullName").GetString());
    }

    [Fact]
    public async Task Register_ShouldReturnBadRequest_WhenRegistrationIsUnsuccessful()
    {
        var request = new RegisterUserRequest("test@test.com", "test", "test", "hashed-password", "User");

        _authServiceMock.Setup(a => a.RegisterUser(request))
            .ReturnsAsync(ResultBuilder.IsFailure<RegisterUserResponse>("Email already in use"));

        var result = await _controller.Register(request);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }
    
    [Fact]
    public async Task Login_Should_Return_Ok_When_Successful()
    {
        var request = new LoginUserRequest("test@test.com", "Password123");
        var response = new LoginUserResponse("token");
        
        _authServiceMock.Setup(a => a.LoginUser(request))
            .ReturnsAsync(ResultBuilder.IsOk(response));
        
        var result = await _controller.Login(request);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var json = JsonSerializer.Serialize(okResult.Value);
        using var doc = JsonDocument.Parse(json);
        var data = doc.RootElement.GetProperty("Data");
        
        Assert.Equal(response.Token, data.GetProperty("Token").GetString());
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
    }

    [Fact]
    public async Task Login_Should_Return_400_When_Failed()
    {
        var request = new LoginUserRequest("test@test.com", "Password123");
        
        _authServiceMock.Setup(a => a.LoginUser(request))
            .ReturnsAsync(ResultBuilder.IsFailure<LoginUserResponse>("Invalid credentials"));
        
        var result = await _controller.Login(request);
        
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var json = JsonSerializer.Serialize(badRequestResult.Value);
        using var doc = JsonDocument.Parse(json);
        var error = doc.RootElement.GetProperty("Message");
        
        Assert.Equal("Invalid credentials", error.GetString());
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }
}