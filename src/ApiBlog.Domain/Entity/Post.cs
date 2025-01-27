using System;
using ApiBlog.Domain.Enums;
using ApiBlog.Domain.Exceptions;

namespace ApiBlog.Domain.Entity;

public class Post : AggregateRoot<string>
{
    public string? Title { get; private set; }
    public string? Content { get; private set; }
    public string? Author { get; private set; }
    public PostStatus Status { get; private set; }
    public DateTimeOffset PublicationDate { get; private set; }
    
    private Post()
    {
    }

    public Post(string title, string content, string author)
    {
        Title = title;
        Content = content;
        Author = author;
        Status = PostStatus.Editable;
    }


    public void Edit(string title, string content)
    {
        if (Status != PostStatus.Editable)
            throw new PostInvalidOperationException(Status);

        Title = title;
        Content = content;
    }

    public void Publish(DateTimeOffset publicationDate )
    {
        if (Status != PostStatus.Editable)
            throw new PostInvalidOperationException(Status);

        Status = PostStatus.Published;
        PublicationDate = publicationDate;
    }
}