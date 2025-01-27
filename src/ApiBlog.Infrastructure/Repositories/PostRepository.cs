using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApiBlog.Domain.Entity;
using ApiBlog.Domain.Repositories;
using MongoDB.Driver;

namespace ApiBlog.Infrastructure.Repositories;

public class PostRepository(IMongoDatabase database) : IPostRepository
{
    private readonly IMongoCollection<Post> _postsCollection = database.GetCollection<Post>("Posts");

    public async Task<IEnumerable<Post>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _postsCollection.Find(p => true).ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<Post> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _postsCollection.Find<Post>(u => u.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddAsync(Post post, CancellationToken cancellationToken = default)
    {
        await _postsCollection.InsertOneAsync(post, null, cancellationToken);
    }

    public async Task UpdateAsync(Post post, CancellationToken cancellationToken = default)
    {
        await _postsCollection.ReplaceOneAsync(p => p.Id == post.Id, post, cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(Post post, CancellationToken cancellationToken = default)
    {
        await _postsCollection.DeleteOneAsync(p => p.Id == post.Id, cancellationToken: cancellationToken);
    }
}