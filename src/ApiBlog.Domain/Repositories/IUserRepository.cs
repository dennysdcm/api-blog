using System.Threading;
using System.Threading.Tasks;
using ApiBlog.Domain.Entity;

namespace ApiBlog.Domain.Repositories;

public interface IUserRepository
{
    Task<User> GetByUserNameAsync(string username, CancellationToken cancellationToken = default);
    Task AddAsync(User user, CancellationToken cancellationToken = default);
}