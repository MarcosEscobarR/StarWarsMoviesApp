using Application.Movies.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Shared.Result;

namespace Application.Movies.Services;

public class MoviesServices(IMovieRepository movieRepository, IMapper mapper)
{
    public async Task<ResultBase<List<MovieResponse>>> GetAll(CancellationToken cancellation)
    {
        try
        {
            var movies = await movieRepository.GetAll(cancellation);
            return ResultBuilder.IsOk(mapper.Map<List<MovieResponse>>(movies));
        }
        catch (Exception e)
        {
            return ResultBuilder.IsFailure<List<MovieResponse>>("An error occurred while fetching movies: " + e.Message);
        }
    }
    
    public async Task<ResultBase<MovieResponse>> GetById(Guid id, CancellationToken cancellation)
    {
        try
        {
            var movie = await movieRepository.GetBy(m => m.Id == id, cancellation);
            if (movie is null)
            {
                return ResultBuilder.IsNotFound<MovieResponse>("Movie not found");
            }
            return ResultBuilder.IsOk(mapper.Map<MovieResponse>(movie));
        }
        catch (Exception e)
        {
            return ResultBuilder.IsFailure<MovieResponse>("An error occurred while fetching the movie: " + e.Message);
        }
    }
    
    public async Task<ResultBase<MovieResponse>> Create(MovieRequest movieRequest, CancellationToken cancellation)
    {
        try
        {
            var movie = mapper.Map<Movie>(movieRequest);
            var createdMovie = await movieRepository.Create(movie, cancellation);
            return ResultBuilder.IsOk(mapper.Map<MovieResponse>(createdMovie));
        }
        catch (Exception e)
        {
            return ResultBuilder.IsFailure<MovieResponse>("An error occurred while creating the movie: " + e.Message);
        }
    }
    
    public async Task<ResultBase<MovieResponse>> Update(Guid id, MovieRequest movieRequest, CancellationToken cancellation)
    {
        try
        {
            var movie = await movieRepository.GetBy(m => m.Id == id, cancellation);
            if (movie is null)
            {
                return ResultBuilder.IsNotFound<MovieResponse>("Movie not found");
            }

            mapper.Map(movieRequest, movie);
            var updatedMovie = await movieRepository.Update(movie, cancellation);
            return ResultBuilder.IsOk(mapper.Map<MovieResponse>(updatedMovie));
        }
        catch (Exception e)
        {
            return ResultBuilder.IsFailure<MovieResponse>("An error occurred while updating the movie: " + e.Message);
        }
    }
    
    public async Task<ResultBase> Delete(Guid id, CancellationToken cancellation)
    {
        try
        {
            var movie = await movieRepository.GetBy(m => m.Id == id, cancellation);
            if (movie is null)
            {
                return ResultBuilder.IsNotFound("Movie not found");
            }
            
            movie.DeletedAt  = DateTime.Now;
            
            await movieRepository.Update(movie, cancellation);
            return ResultBuilder.IsOk();
        }
        catch (Exception e)
        {
            return ResultBuilder.IsFailure("An error occurred while deleting the movie: " + e.Message);
        }
    }
}