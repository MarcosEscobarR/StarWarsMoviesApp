using Application.Movies.DTOs;
using Shared.Result;

namespace Application.Movies.Services;

public interface IMoviesServices
{
    Task<ResultBase<List<MovieResponse>>> GetAll(CancellationToken cancellation);
    Task<ResultBase<MovieResponse>> GetById(Guid id, CancellationToken cancellation);
    Task<ResultBase<MovieResponse>> Create(MovieRequest movieRequest, CancellationToken cancellation);
    Task<ResultBase<MovieResponse>> Update(Guid id, MovieRequest movieRequest, CancellationToken cancellation);
    Task<ResultBase> Delete(Guid id, CancellationToken cancellation);
}