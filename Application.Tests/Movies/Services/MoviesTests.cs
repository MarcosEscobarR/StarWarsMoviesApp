using System.Linq.Expressions;
using System.Net;
using Application.Movies.DTOs;
using Application.Movies.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace Application.Tests.Movies.Services;

public class MoviesTests
{
    private readonly Mock<IMovieRepository> _movieRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    
    private readonly MoviesServices _moviesService;

    public MoviesTests()
    {
        _movieRepositoryMock = new Mock<IMovieRepository>();
        _mapperMock = new Mock<IMapper>();
        
        _moviesService = new MoviesServices(_movieRepositoryMock.Object, _mapperMock.Object);
    }
    
       [Fact]
    public async Task GetAll_ShouldReturnMoviesSuccessfully()
    {
        var movies = new List<Movie> { new() { Id = Guid.NewGuid(), Title = "Test Movie" } };
        var movieResponses = new List<MovieResponse> { new(Id: movies[0].Id, Title: "Test Movie", Producer: "Test Producer", Director: "test director", ReleaseDate: DateTime.Now) { } };

        _movieRepositoryMock.Setup(r => r.GetAll(It.IsAny<CancellationToken>())).ReturnsAsync(movies);
        _mapperMock.Setup(m => m.Map<List<MovieResponse>>(movies)).Returns(movieResponses);

        var result = await _moviesService.GetAll(CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(movieResponses, result.Data);
    }

    [Fact]
    public async Task GetAll_ShouldHandleException()
    {
        _movieRepositoryMock.Setup(r => r.GetAll(It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("DB error"));

        var result = await _moviesService.GetAll(CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Contains("DB error", result.Message);
    }

    [Fact]
    public async Task GetById_ShouldReturnMovieSuccessfully()
    {
        var movie = new Movie { Id = Guid.NewGuid(), Title = "Test" };
        var response = new MovieResponse (Id: movie.Id, Title: "Test Movie", Producer: "Test Producer", Director: "test director", ReleaseDate: DateTime.Now) {};

        _movieRepositoryMock.Setup(r => r.GetBy(It.IsAny<Expression<Func<Movie, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(movie);
        _mapperMock.Setup(m => m.Map<MovieResponse>(movie)).Returns(response);

        var result = await _moviesService.GetById(movie.Id, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(response, result.Data);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound()
    {
        _movieRepositoryMock.Setup(r => r.GetBy(It.IsAny<Expression<Func<Movie, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync((Movie)null);

        var result = await _moviesService.GetById(Guid.NewGuid(), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal( 404, result.StatusCode);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedMovie()
    {
        var request = new MovieRequest("Test Movie", "Test Producer", "Test Director", DateTime.Now);
        var movie = new Movie { Id = Guid.NewGuid(), Title = request.Title };
        var response = new MovieResponse(Id: movie.Id, Title: "Test Movie", Producer: "Test Producer", Director: "test director", ReleaseDate: DateTime.Now) { };   

        _mapperMock.Setup(m => m.Map<Movie>(request)).Returns(movie);
        _movieRepositoryMock.Setup(r => r.Create(movie, It.IsAny<CancellationToken>())).ReturnsAsync(movie);
        _mapperMock.Setup(m => m.Map<MovieResponse>(movie)).Returns(response);

        var result = await _moviesService.Create(request, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(response, result.Data);
    }

    [Fact]
    public async Task Create_ShouldHandleException()
    {
        var request = new MovieRequest("Test Movie", "Test Producer", "Test Director", DateTime.Now);

        _mapperMock.Setup(m => m.Map<Movie>(request)).Throws(new Exception("Map error"));

        var result = await _moviesService.Create(request, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Contains("Map error", result.Message);
    }

    [Fact]
    public async Task Update_ShouldReturnUpdatedMovie()
    {
        var id = Guid.NewGuid();
        var request = new MovieRequest("Test Movie", "Test Producer", "Test Director", DateTime.Now);
        var movie = new Movie { Id = id, Title = "Old" };
        var updatedMovie = new Movie { Id = id, Title = "Updated" };
        var response = new MovieResponse(Id: updatedMovie.Id, Title: "Updated", Producer: "Test Producer", Director: "test director", ReleaseDate: DateTime.Now) { };

        _movieRepositoryMock.Setup(r => r.GetBy(It.IsAny<Expression<Func<Movie, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(movie);
        _mapperMock.Setup(m => m.Map(request, movie)).Callback(() => movie.Title = request.Title);
        _movieRepositoryMock.Setup(r => r.Update(movie, It.IsAny<CancellationToken>())).ReturnsAsync(movie);
        _mapperMock.Setup(m => m.Map<MovieResponse>(movie)).Returns(response);

        var result = await _moviesService.Update(id, request, CancellationToken.None);
        var data = (MovieResponse)result.Data!;
        Assert.True(result.IsSuccess);
        Assert.Equal("Updated", data.Title);
    }

    [Fact]
    public async Task Update_ShouldReturnNotFound()
    {
        var request = new MovieRequest("Test Movie", "Test Producer", "Test Director", DateTime.Now);

        _movieRepositoryMock.Setup(r => r.GetBy(It.IsAny<Expression<Func<Movie, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync((Movie)null);

        var result = await _moviesService.Update(Guid.NewGuid(), request, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task Delete_ShouldSoftDeleteMovie()
    {
        var movie = new Movie { Id = Guid.NewGuid(), Title = "Test" };

        _movieRepositoryMock.Setup(r => r.GetBy(It.IsAny<Expression<Func<Movie, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(movie);
        _movieRepositoryMock.Setup(r => r.Update(movie, It.IsAny<CancellationToken>())).ReturnsAsync(movie);

        var result = await _moviesService.Delete(movie.Id, CancellationToken.None);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound()
    {
        _movieRepositoryMock.Setup(r => r.GetBy(It.IsAny<Expression<Func<Movie, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync((Movie)null);

        var result = await _moviesService.Delete(Guid.NewGuid(), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(404, result.StatusCode);
    }
}