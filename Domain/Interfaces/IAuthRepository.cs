using System.Linq.Expressions;
using Domain.Entities;

namespace Domain.Interfaces;


public interface IAuthRepository
{
    public Task<User> CreateUser(User user, CancellationToken cancellationToken = default);
    public Task<User> GetUserBy(Expression<Func<User, bool>> predicate, CancellationToken cancellation = default);

}