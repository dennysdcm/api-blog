using ApiBlog.Application.Dtos;
using ApiBlog.Application.Exceptions;
using ApiBlog.Application.Services;
using ApiBlog.Domain.Entity;
using ApiBlog.Domain.Enums;
using ApiBlog.Domain.Repositories;
using FluentAssertions;
using NSubstitute;

namespace ApiBlog.UnitTests.Application.Services;

public class PostServiceTests
{
    private readonly IPostRepository _postRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserContext _currentUserContext;
    
    private readonly IPostsService _sut;

    public PostServiceTests()
    {
        _postRepository = Substitute.For<IPostRepository>();
        _dateTimeProvider = Substitute.For<IDateTimeProvider>();
        _currentUserContext = Substitute.For<ICurrentUserContext>();
        
        _sut = new PostService(_postRepository, _dateTimeProvider, _currentUserContext);
    }
    
    [Fact]
    public async Task CreatePostAsync__Should_Create_A_New_Post()
    {
        _currentUserContext.Username.Returns("test_user");

        await _sut.CreatePostAsync(new CreatePostRequest("title", "content"));

        await _postRepository.Received(1).AddAsync(Arg.Any<Post>());
    }
    
    [Fact]
    public async Task EditPostAsync__Should_Edit_A_Post_Content()
    {
        var post = new Post("title", "content", "author");
        _postRepository.GetByIdAsync(Arg.Any<string>()).Returns(Task.FromResult(post));

        await _sut.EditPostAsync(new EditPostRequest("abcdef", "new title", "new content"));

        post.Title.Should().Be("new title");
        post.Content.Should().Be("new content");
        post.Status.Should().Be(PostStatus.Editable);
        
        await _postRepository.Received(1).UpdateAsync(Arg.Any<Post>());
    }

    [Fact]
    public async Task EditPostAsync__Should_Throw_PostNotFoundException_When_Post_Is_Not_Found()
    {
        _postRepository.GetByIdAsync(Arg.Any<string>()).Returns(Task.FromResult<Post?>(default));

        var handle = async () => await _sut.EditPostAsync(new EditPostRequest("abcdef", "new title", "new content"));

        await handle.Should().ThrowAsync<PostNotFoundException>();
        await _postRepository.Received(0).UpdateAsync(Arg.Any<Post>());
    }
    
    [Fact]
    public async Task DeletePostAsync__Should_Delete_A_Post()
    {
        var post = new Post("title", "content", "author");
        _postRepository.GetByIdAsync(Arg.Any<string>()).Returns(Task.FromResult(post));

        await _sut.DeletePostAsync("asbcde");
        
        await _postRepository.Received(1).DeleteAsync(Arg.Any<Post>());
    }
    
    [Fact]
    public async Task PublishPostAsync__Should_Publish_A_Post()
    {
        var post = new Post("title", "content", "author");
        _dateTimeProvider.UtcNow.Returns(new DateTime(2020, 1, 1));
        _postRepository.GetByIdAsync(Arg.Any<string>()).Returns(Task.FromResult(post));

        await _sut.PublishPostAsync("abcdef");

        post.Status.Should().Be(PostStatus.Published);
        post.PublicationDate.Should().Be(new DateTime(2020, 1, 1));
        
        await _postRepository.Received(1).UpdateAsync(Arg.Any<Post>());
    }
    
    [Fact]
    public async Task PublishPostAsync__Should_Throw_PostNotFoundException_When_Post_Is_Not_Found()
    {
        
        _postRepository.GetByIdAsync(Arg.Any<string>()).Returns(Task.FromResult<Post?>(default));

        var handle = async () => await _sut.PublishPostAsync("abcdef");

        await handle.Should().ThrowAsync<PostNotFoundException>();
        await _postRepository.Received(0).UpdateAsync(Arg.Any<Post>());
    }
}