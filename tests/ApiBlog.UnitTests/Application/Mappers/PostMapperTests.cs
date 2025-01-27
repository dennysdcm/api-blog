using ApiBlog.Application.Mappers;
using ApiBlog.Domain.Entity;
using FluentAssertions;

namespace ApiBlog.UnitTests.Application.Mappers;

public class PostMapperTests
{
    [Fact]
    public void AsDto__Should_Return_A_PostDto()
    {
        var post = CreatePublishedPost();

        var postDto = post.AsDto();

        postDto.Id.Should().Be(post.Id);
        postDto.Title.Should().Be(post.Title);
        postDto.Content.Should().Be(post.Content);
        postDto.Author.Should().Be(post.Author);
        postDto.Status.Should().Be(post.Status.ToString());
        postDto.PublicationDate.Should().Be(post.PublicationDate);
    }
    
    private Post CreatePublishedPost()
    {
        var post = new Post("title", "content", "author");
        post.Publish(DateTimeOffset.UtcNow);
        
        return post;
    }
}