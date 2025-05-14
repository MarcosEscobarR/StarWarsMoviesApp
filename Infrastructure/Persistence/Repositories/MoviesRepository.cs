using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class MoviesRepository(AppDbContext context) : IMovieRepository
{
    public Task<List<Movie>> GetAll(CancellationToken cancellation)
    {
        return context.Movies.ToListAsync(cancellation);
    }

    public Task<Movie> GetBy(Expression<Func<Movie, bool>> predicate, CancellationToken cancellation)
    {
        return context.Movies
            .AsNoTracking()
            .FirstOrDefaultAsync(predicate, cancellation);
    }

    public async Task<Movie> Create(Movie movie, CancellationToken cancellation)
    {
        var result = context.Movies.Add(movie);
        await SaveChanges(cancellation);
        return result.Entity;
    }

    public async Task<Movie> Update(Movie movie, CancellationToken cancellation)
    {
        var result = context.Movies.Update(movie);
        await SaveChanges(cancellation);
        return result.Entity;
    }

    public async Task<bool> Delete(Movie movie, CancellationToken cancellation)
    {
        movie.DeletedAt = DateTime.Now;
        return await SaveChanges(cancellation);
    }
    
    private async Task<bool> SaveChanges(CancellationToken cancellation)
    {
        return await context.SaveChangesAsync(cancellation) > 0;
    }
}