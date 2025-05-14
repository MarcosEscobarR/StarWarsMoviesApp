using Domain.Entities;
using Domain.Interfaces;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class AuthRepository(AppDbContext context) : IAuthRepository
{
    public async Task<User> CreateUser(User user, CancellationToken cancellation = default)
    {
        var result = await context.Users.AddAsync(user, cancellation);
        await SaveChangesAsync(cancellation);
        return result.Entity;
    }
    
    public async Task<User> GetUserBy(Expression<Func<User, bool>> predicate, CancellationToken cancellation = default)
    {
        return await context.Users.FirstOrDefaultAsync(predicate, cancellation);
    }

    private async Task SaveChangesAsync(CancellationToken cancellation = default)
    {
        await context.SaveChangesAsync(cancellation);
    }
}