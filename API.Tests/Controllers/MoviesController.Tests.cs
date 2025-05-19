using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Application.Movies.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Moq;

namespace API.Tests.Controllers;

public class MoviesControllerTests : IClassFixture<CustomWebApplicationMovieFactory>
{
    private readonly CustomWebApplicationMovieFactory _factory;
    private readonly HttpClient _client;

    public MoviesControllerTests(CustomWebApplicationMovieFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    private string GenerateJwtToken(string role)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("THIS_IS_A_SUPER_SECRET_KEY_1234567890")); // Usa la misma que en tus settings
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, "testuser"),
            new Claim(ClaimTypes.Role, role)
        };

        var token = new JwtSecurityToken(
            issuer: "StarWarsAPI",
            audience: "StarWarsClient",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    [Fact]
    public async Task UpdateMovie_Should_Return401_WhenNoToken()
    {
        var response = await _client.PutAsync("/api/Movies/00000000-0000-0000-0000-000000000001", new StringContent("{}"));
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task UpdateMovie_Should_Return403_WhenUserIsUser()
    {
        var token = GenerateJwtToken("User");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var request = new MovieRequest("title", "prod", "dir", DateTime.Now);
        var response = await _client.PutAsync("/api/Movies/00000000-0000-0000-0000-000000000001",
            new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));

        Assert.Equal(System.Net.HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task UpdateMovie_Should_Return200_WhenUserIsAdmin()
    {
        var token = GenerateJwtToken("Admin");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var movie = new Movie
        {
            Id = Guid.NewGuid(),
            Title = "Old Title",
            Producer = "Prod",
            Director = "Dir",
            ReleaseDate = DateTime.Now
        };

        _factory.MovieRepositoryMock.Setup(r =>
                r.GetBy(It.IsAny<Expression<Func<Movie, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(movie);
        _factory.MovieRepositoryMock.Setup(r =>
                r.Update(It.IsAny<Movie>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(movie);

        var request = new MovieRequest("Updated Title", "Prod", "Dir", DateTime.Now);

        var response = await _client.PutAsync($"/api/Movies/{movie.Id}",
            new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));

        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task DeleteMovie_Should_Return401_WhenNoToken()
    {
        var response = await _client.DeleteAsync("/api/Movies/00000000-0000-0000-0000-000000000001");
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task DeleteMovie_Should_Return403_WhenUserIsUser()
    {
        var token = GenerateJwtToken("User");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync("/api/Movies/00000000-0000-0000-0000-000000000001");
        Assert.Equal(System.Net.HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task DeleteMovie_Should_Return200_WhenUserIsAdmin()
    {
        var token = GenerateJwtToken("Admin");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var movie = new Movie
        {
            Id = Guid.NewGuid(),
            Title = "To Delete",
            Producer = "Prod",
            Director = "Dir",
            ReleaseDate = DateTime.Now
        };

        _factory.MovieRepositoryMock.Setup(r =>
                r.GetBy(It.IsAny<Expression<Func<Movie, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(movie);
        _factory.MovieRepositoryMock.Setup(r =>
                r.Update(It.IsAny<Movie>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(movie);

        var response = await _client.DeleteAsync($"/api/Movies/{movie.Id}");

        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }
}

public class CustomWebApplicationMovieFactory : WebApplicationFactory<Program>
{
    public Mock<IMovieRepository> MovieRepositoryMock { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IMovieRepository));
            if (descriptor != null)
                services.Remove(descriptor);

            services.AddSingleton(MovieRepositoryMock.Object);
        });
    }
}