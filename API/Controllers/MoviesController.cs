using API.Extensions;
using Application.Movies.DTOs;
using Application.Movies.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController(MoviesServices moviesServices) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetMovies(CancellationToken cancellationToken)
    {
        var result = await moviesServices.GetAll(cancellationToken);
        return result.ToActionResult();
    }
    
    [HttpGet("{id:guid}")]
    [Authorize(Roles = nameof(UserRole.User))]
    public async Task<IActionResult> GetMovie(Guid id, CancellationToken cancellation)
    {
        var result = await moviesServices.GetById(id, cancellation);
        return result.ToActionResult();
    }
    
    [HttpPost]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> AddMovie(MovieRequest request, CancellationToken cancellation)
    {
        var result = await moviesServices.Create(request, CancellationToken.None);
        return result.ToActionResult();
    }
    
    [HttpPut("{id:guid}")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> UpdateMovie(Guid id, MovieRequest request, CancellationToken cancellation)
    {
        var result = await moviesServices.Update(id, request, CancellationToken.None);
        return result.ToActionResult();
    }
    
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> DeleteMovie(Guid id, CancellationToken cancellation)
    {
        var result = await moviesServices.Delete(id, cancellation);
        return result.ToActionResult();
    }
}