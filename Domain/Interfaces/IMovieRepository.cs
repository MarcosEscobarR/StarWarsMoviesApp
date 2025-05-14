using System.Linq.Expressions;
using Domain.Entities;

namespace Domain.Interfaces;

public interface IMovieRepository
{
    public Task<List<Movie>> GetAll(CancellationToken cancellation);
    public Task<Movie?> GetBy(Expression<Func<Movie, bool>> predicate, CancellationToken cancellation);
    public Task<Movie> Create(Movie movie, CancellationToken cancellation);
    public Task<Movie> Update(Movie movie, CancellationToken cancellation);
    public Task<bool> Delete(Movie movie, CancellationToken cancellation);
}