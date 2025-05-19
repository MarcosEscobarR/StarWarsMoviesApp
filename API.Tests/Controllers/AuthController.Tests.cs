using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using Application.Auth.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace API.Tests.Controllers;

public class AuthControllerTests: IClassFixture<CustomWebApplicationAuthFactory>
{
    private readonly CustomWebApplicationAuthFactory _factory;
    private readonly HttpClient _client;
    
    public AuthControllerTests(CustomWebApplicationAuthFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task Login_Should_Return_Ok_When_Valid_Credentials()
    {
        var loginRequest = new LoginUserRequest("test@test.com", "hashed-password");
    
        var existingUser = new User(
            "testuser",
            "testuser",
            "test@test.com",
            "hashed-password"
        );

        _factory.AuthRepositoryMock
            .Setup(r => r.GetUserBy(It.IsAny<Expression<Func<User, bool>>>(), default))
            .ReturnsAsync(existingUser);
        _factory.PasswordHasherMock
            .Setup(repo => repo.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(PasswordVerificationResult.Success);

        var response = await _client.PostAsync("/api/auth/login",new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json"));

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Register_Should_Return_Ok_When_Valid_Request()
    {
        var registerRequest = new RegisterUserRequest("testuser", "testuser", "test", "password", "User");
        
        
    }
}

public class CustomWebApplicationAuthFactory : WebApplicationFactory<Program>
{
    public Mock<IAuthRepository> AuthRepositoryMock { get; } = new();
    public Mock<IPasswordHasher<User>> PasswordHasherMock { get; } = new();


    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IAuthRepository)); 
            if (descriptor != null)
                services.Remove(descriptor);

            services.AddSingleton(AuthRepositoryMock.Object);
            services.AddSingleton(PasswordHasherMock.Object);
        });
    }
}