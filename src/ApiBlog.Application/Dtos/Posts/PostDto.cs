using System;

namespace ApiBlog.Application.Dtos;

public class PostDto
{
    public string? Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? Author { get; set; }
    public string? Status { get; set; }
    public DateTimeOffset? PublicationDate { get; set; }
}