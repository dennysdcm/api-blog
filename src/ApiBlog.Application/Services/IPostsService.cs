using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApiBlog.Application.Dtos;
using ApiBlog.Application.Exceptions;
using ApiBlog.Application.Mappers;
using ApiBlog.Domain.Entity;
using ApiBlog.Domain.Repositories;

namespace ApiBlog.Application.Services;

public interface IPostsService
{
    Task<IEnumerable<PostDto>> GetPostsAsync(CancellationToken cancellationToken = default);
    Task<PostDto> GetPostByIdAsync(string id, CancellationToken cancellationToken = default);
    Task CreatePostAsync(CreatePostRequest request, CancellationToken cancellationToken = default);
    Task EditPostAsync(EditPostRequest request, CancellationToken cancellationToken = default);
    Task DeletePostAsync(string id, CancellationToken cancellationToken = default);
    Task PublishPostAsync(string id, CancellationToken cancellationToken = default);
}

public class PostService : IPostsService
{
    private readonly IPostRepository _postRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserContext _currentUserContext;

    public PostService(IPostRepository postRepository, IDateTimeProvider dateTimeProvider, ICurrentUserContext currentUserContext)
    {
        _postRepository = postRepository;
        _dateTimeProvider = dateTimeProvider;
        _currentUserContext = currentUserContext;
    }

    public async Task<IEnumerable<PostDto>> GetPostsAsync(CancellationToken cancellationToken = default)
    {
        return (await _postRepository.GetAllAsync(cancellationToken)).Select(p => p.AsDto());
    }

    public async Task<PostDto> GetPostByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return (await _postRepository.GetByIdAsync(id, cancellationToken)).AsDto();
    }

    public async Task CreatePostAsync(CreatePostRequest request, CancellationToken cancellationToken = default)
    {
        var post = new Post(request.Title, request.Content, _currentUserContext.Username ?? string.Empty);
        await _postRepository.AddAsync(post, cancellationToken);
    }

    public async Task EditPostAsync(EditPostRequest request, CancellationToken cancellationToken = default)
    {
        var post = await _postRepository.GetByIdAsync(request.Id, cancellationToken);
        if (post == null) throw new PostNotFoundException(request.Id);
        
        post.Edit(request.Title, request.Content);
        await _postRepository.UpdateAsync(post, cancellationToken);
    }

    public async Task DeletePostAsync(string id, CancellationToken cancellationToken = default)
    {
        var post = await _postRepository.GetByIdAsync(id, cancellationToken);
        if (post == null) throw new PostNotFoundException(id);
        
        await _postRepository.DeleteAsync(post, cancellationToken);
    }

    public async Task PublishPostAsync(string id, CancellationToken cancellationToken = default)
    {
        var post = await _postRepository.GetByIdAsync(id, cancellationToken);
        if (post == null) throw new PostNotFoundException(id);
        
        post.Publish(_dateTimeProvider.UtcNow);
        await _postRepository.UpdateAsync(post, cancellationToken);
    }
}