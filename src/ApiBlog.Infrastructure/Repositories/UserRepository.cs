using System.Threading;
using System.Threading.Tasks;
using ApiBlog.Domain.Entity;
using ApiBlog.Domain.Repositories;
using MongoDB.Driver;

namespace ApiBlog.Infrastructure.Repositories;

public class UserRepository(IMongoDatabase database) : IUserRepository
{
    private readonly IMongoCollection<User> _usersCollection = database.GetCollection<User>("Users");

    public async Task<User> GetByUserNameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _usersCollection.Find<User>(u => u.Username == username).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _usersCollection.InsertOneAsync(user, null, cancellationToken);
    }
}