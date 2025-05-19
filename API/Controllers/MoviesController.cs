using API.Extensions;
using Application.Movies.DTOs;
using Application.Movies.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Result;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController(IMoviesServices moviesServices) : ControllerBase
{
    /// <summary>
    /// Descripción de la operación: Obtiene una lista de todas las películas.
    /// </summary>
    /// <returns>La lista de peliculas no eliminadas</returns>
    /// <response code="200">Peticion Exitosa.</response>
    /// <response code="500">Error interno del servidor.</response>
    [HttpGet]
    [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultBase), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResultBase), StatusCodes.Status500InternalServerError)]    public async Task<IActionResult> GetMovies(CancellationToken cancellationToken)
    {
        var result = await moviesServices.GetAll(cancellationToken);
        return result.ToActionResult();
    }
    
    
    /// <summary>
    /// Descripción de la operación: Obtiene una película por su ID [USER].
    /// </summary>
    /// <param name="id">>El ID de la película a obtener</param>
    /// <returns>La descripcion de la pelicula</returns>
    /// <response code="200">Peticion Exitosa.</response>
    /// <response code="400">Error de validación o de negocio.</response>
    /// <response code="403">Usuario no autorizado</response>
    /// <response code="404">No se encontró la película.</response>
    /// <response code="500">Error interno del servidor.</response>
    [HttpGet("{id:guid}")]

    [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultBase), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResultBase), StatusCodes.Status500InternalServerError)]  [Authorize(Roles = nameof(UserRole.User))]
    public async Task<IActionResult> GetMovie(Guid id, CancellationToken cancellation)
    {
        var result = await moviesServices.GetById(id, cancellation);
        return result.ToActionResult();
    }
    
    
    /// <summary>
    /// Descripción de la operación: Crea una nueva película [ADMIN].
    /// </summary>
    /// <param name="request">Los datos de la nueva pelicula.</param>
    /// <returns>Informacion de la pelicula creada</returns>
    /// <response code="200">Peticion Exitosa.</response>
    /// <response code="400">Error de validación o de negocio.</response>
    /// <response code="403">Usuario no autorizado</response>
    /// <response code="500">Error interno del servidor.</response>
    [HttpPost]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultBase), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResultBase), StatusCodes.Status500InternalServerError)] 
    public async Task<IActionResult> AddMovie(MovieRequest request, CancellationToken cancellation)
    {
        var result = await moviesServices.Create(request, CancellationToken.None);
        return result.ToActionResult();
    }
    
    /// <summary>
    /// Descripción de la operación: Actualiza una pelicula existente [ADMIN].
    /// </summary>
    /// <param name="id">>El ID de la película a obtener</param>
    /// <param name="request">Los datos de la pelicula a actualizar.</param>
    /// <returns>Informacion de la pelicula actualizada</returns>
    /// <response code="200">Peticion Exitosa.</response>
    /// <response code="400">Error de validación o de negocio.</response>
    /// <response code="403">Usuario no autorizado</response>
    /// <response code="404">No se encontró la película.</response>
    /// <response code="500">Error interno del servidor.</response>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultBase), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResultBase), StatusCodes.Status500InternalServerError)] 
    public async Task<IActionResult> UpdateMovie(Guid id, MovieRequest request, CancellationToken cancellation)
    {
        var result = await moviesServices.Update(id, request, CancellationToken.None);
        return result.ToActionResult();
    }
    
    
    /// <summary>
    /// Descripción de la operación: Elimina una pelicula por su ID [ADMIN].
    /// </summary>
    /// <param name="id">>El ID de la película a eliminar</param>
    /// <returns>Response Basico.</returns>
    /// <response code="200">Peticion Exitosa.</response>
    /// <response code="400">Error de validación o de negocio.</response>
    /// <response code="403">Usuario no autorizado</response>
    /// <response code="404">No se encontró la película.</response>
    /// <response code="500">Error interno del servidor.</response>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [ProducesResponseType(typeof(ResultBase), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultBase), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResultBase), StatusCodes.Status500InternalServerError)] 
    public async Task<IActionResult> DeleteMovie(Guid id, CancellationToken cancellation)
    {
        var result = await moviesServices.Delete(id, cancellation);
        return result.ToActionResult();
    }
}