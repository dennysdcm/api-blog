using ApiBlog.Domain.Entity;
using ApiBlog.Domain.Enums;
using ApiBlog.Domain.Exceptions;
using FluentAssertions;

namespace ApiBlog.UnitTests.Domain.Entity;

public class PostTest
{
    [Fact]
    public void Edit__Should_Edit_Post_When_Post_Is_In_Editable_State()
    {
        var newTitle = "new Title";
        var newContent = "new Content";
        var sut = CreateNewPost();

        sut.Edit(newTitle, newContent);

        sut.Title.Should().Be(newTitle);
        sut.Content.Should().Be(newContent);
    }

    [Fact]
    public void Edit__Should_Throw_PostInvalidOperationException_When_Post_Is_Not_In_Editable_State()
    {
        var newTitle = "new Title";
        var newContent = "new Content";
        var sut = CreatePublishedPost();

        var edit = () => sut.Edit(newTitle, newContent);

        edit.Should().Throw<PostInvalidOperationException>();
    }


    [Fact]
    public void Publish__Should_Set_To_Published_State()
    {
        var pubicationDate = DateTime.Now;
        
        var sut = CreateNewPost();
        sut.Publish(pubicationDate);

        sut.Status.Should().Be(PostStatus.Published);
        sut.PublicationDate.Should().Be(pubicationDate);
    }

    [Fact]
    public void Publish__Should_Throw_PostInvalidOperationException_When_Post_Is_Already_In_Publish_State()
    {
        var sut = CreatePublishedPost();

        var secondSubmit = () => sut.Publish(DateTime.Now);;

        secondSubmit.Should().Throw<PostInvalidOperationException>();
    }

    private Post CreateNewPost()
    {
        return new Post("title", "content", "author");
    }
    
    private Post CreatePublishedPost()
    {
        var post = new Post("title", "content", "author");
        post.Publish(DateTimeOffset.UtcNow);
        
        return post;
    }
}