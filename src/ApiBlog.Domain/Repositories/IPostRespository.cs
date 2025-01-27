using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApiBlog.Domain.Entity;

namespace ApiBlog.Domain.Repositories;

public interface IPostRepository
{
    Task<IEnumerable<Post>> GetAllAsync(CancellationToken cancellationToken);
    Task<Post> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task AddAsync(Post post, CancellationToken cancellationToken = default);
    Task UpdateAsync(Post post, CancellationToken cancellationToken = default);
    Task DeleteAsync(Post post, CancellationToken cancellationToken = default);
}