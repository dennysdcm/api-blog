using ApiBlog.Application.Dtos;
using ApiBlog.Domain.Entity;

namespace ApiBlog.Application.Mappers;

public static class PostMappers
{
    public static PostDto AsDto(this Post post)
    {
        return new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            Author = post.Author,
            Status = post.Status.ToString(),
            PublicationDate = post.PublicationDate
        };
    }
}