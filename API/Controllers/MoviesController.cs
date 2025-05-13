using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController: ControllerBase
{
    [HttpGet]
    public Task<IActionResult> GetMovies()
    {
        return Task.FromResult<IActionResult>(Ok("List of movies"));
    }
    
    [HttpGet("{id}")]
    public Task<IActionResult> GetMovie(int id)
    {
        return Task.FromResult<IActionResult>(Ok($"Details of movie with ID {id}"));
    }
    
    [HttpPost]
    public Task<IActionResult> AddMovie()
    {
        return Task.FromResult<IActionResult>(Ok("Movie added successfully"));
    }
    
    [HttpPut("{id}")]
    public Task<IActionResult> UpdateMovie(int id)
    {
        return Task.FromResult<IActionResult>(Ok($"Movie with ID {id} updated successfully"));
    }
    
    [HttpDelete("{id}")]
    public Task<IActionResult> DeleteMovie(int id)
    {
        return Task.FromResult<IActionResult>(Ok($"Movie with ID {id} deleted successfully"));
    }
}